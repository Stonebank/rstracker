using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using rstracker.Core;
using rstracker.Models;
using System.Net;

namespace rstracker.Controllers
{
    public class HighscoreController : Controller
    {

        public async Task<IActionResult> RS3Async(string username)
        {

            if (string.IsNullOrEmpty(username))
                return BadRequest("The given input is either empty or null.");

            var player = DataHandler.GetPlayer(username);
            if (player is null)
                return BadRequest($"The player \"{username}\" was not found. This could be because their profile is private or the username input is incorrect.");

            long lastUpdate = player.LastUpdate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (lastUpdate > 0 && player.SkillData.Count > 0)
                return View("rs3/profile", player);

            if (DateTime.Today.Date > player.LastResetDay.Date)
            {
                player.LastResetDay = DateTime.Today;
                player.DailyTrackingData.Clear();
                if (player.GameModeModel.GameMode == Enums.GameMode.HARDCORE_IRONMAN || player.GameModeModel.GameMode == Enums.GameMode.ULTIMATE_IRONMAN)
                    await DetermineGameMode(player).ConfigureAwait(false);
            }

            if (!Constants.PLAYERS.Contains(player))
                Constants.PLAYERS.Add(player);

            if (player.GameModeModel.GameMode == Enums.GameMode.NONE)
                await DetermineGameMode(player).ConfigureAwait(false);

            player.SearchCount++;

            if (player.LastSkillDataUpdate.Count > 0)
                player.LastSkillDataUpdate.Clear();

            player.LastSkillDataUpdate = new List<string>(player.SkillData);
            player.SkillData.Clear();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(Constants.GetHighscoreEndPoint(username)).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound)
                    return BadRequest($"The player \"{username}\" was not found. This could be because their profile is private or the username input is incorrect.");

                string data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                string[] split = data.Split("\n");
                foreach (var item in split)
                {
                    string[] parts = item.Split(',');
                    if (parts.Length != 3)
                        continue;
                    player.SkillData.Add(item);
                }
            }

            await FetchRS3ClanData(player).ConfigureAwait(false);

            player.AppendDailyXp();
            player.LastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 60000;

            DataHandler.Save(player);

            return View("rs3/profile", player);
        }

        private static async Task FetchRS3ClanData(PlayerModel player)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Constants.GetClanDataEndPoint(player.Username)).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound)
                    return;

                string data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string json = ClanMemberModel.ExtractJsonData(data);

                if (string.IsNullOrEmpty(json))
                    return;

                var member = JsonConvert.DeserializeObject<ClanMemberModel>(json);
                if (member is null)
                    return;

                player.ClanMemberModel = new ClanMemberModel(member.IsSuffix, member.Recruiting, member.Name, member.Clan, member.Title, member.World, member.Online);
            }
        }

        private static async Task DetermineGameMode(PlayerModel player)
        {
            using (var client = new HttpClient())
            {

                var tasks = new[]
                {
                    client.GetAsync(Constants.GetHardcoreIronmanEndPoint(player.Username)),
                    client.GetAsync(Constants.GetIronmanEndPoint(player.Username))
                };

                await Task.WhenAll(tasks);

                foreach (var response in tasks)
                {
                    if (response.Result.IsSuccessStatusCode && response.Result.StatusCode == HttpStatusCode.OK)
                    {
                        response.Result.EnsureSuccessStatusCode();

                        if (response == tasks[0])
                        {

                            var web = new HtmlWeb();
                            var document = await web.LoadFromWebAsync(Constants.GetHardcoreIronmanHighscore(player.Username));

                            string xpath = $"//td[a[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{player.Username.ToLower()}')]]/a/div[@class='death-icon']";
                            var deathIconNode = document.DocumentNode.SelectSingleNode(xpath);

                            if (deathIconNode is not null)
                            {

                                var deathIconDetailsNode = deathIconNode.SelectSingleNode(".//div[@class='death-icon__details']");

                                var dateNode = deathIconDetailsNode.SelectSingleNode("./p[@class='death-icon__date']");
                                if (dateNode is not null)
                                    player.GameModeModel.DeathDate = dateNode.InnerText;

                                var locationNode = deathIconDetailsNode.SelectSingleNode("./p[@class='death-icon__location']");
                                if (locationNode is not null)
                                    player.GameModeModel.DeathLocation = locationNode.InnerText;

                                var descNode = deathIconDetailsNode.SelectSingleNode("./p[@class='death-icon__desc']");
                                if (descNode is not null)
                                    player.GameModeModel.DeathReason = descNode.InnerText;

                                player.GameModeModel.GameMode = Enums.GameMode.IRONMAN;
                                break;
                            }
                            
                        }

                        player.GameModeModel.GameMode = response == tasks[0] ? Enums.GameMode.HARDCORE_IRONMAN : Enums.GameMode.IRONMAN;
                        break;
                    }
                    player.GameModeModel.GameMode = Enums.GameMode.REGULAR;
                }
            }
        }

    }
}
