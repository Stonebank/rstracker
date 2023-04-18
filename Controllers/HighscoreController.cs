using Microsoft.AspNetCore.Mvc;
using rstracker.Core;
using System.Net;

namespace rstracker.Controllers
{
    public class HighscoreController : Controller
    {

        public IActionResult RS3(string username)
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
            }

            if (!Constants.PLAYERS.Contains(player))
                Constants.PLAYERS.Add(player);

            player.SearchCount++;

            if (player.LastSkillDataUpdate.Count > 0)
                player.LastSkillDataUpdate.Clear();

            player.LastSkillDataUpdate = new List<string>(player.SkillData);
            player.SkillData.Clear();

            string url = $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={username}";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound)
                    return BadRequest($"The player \"{username}\" was not found. This could be because their profile is private or the username input is incorrect.");
                
                string data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                string[] split = data.Split("\n");
                foreach (var item in split)
                {
                    string[] parts = item.Split(',');
                    if (parts.Length != 3)
                        continue;
                    player.SkillData.Add(item);
                }
            }

            player.AppendDailyXp();
            player.LastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 60000;

            DataHandler.Save(player);

            return View("rs3/profile", player);
        }


    }
}
