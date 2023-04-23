using Newtonsoft.Json;
using rstracker.Models;
using rstracker.Utility;

namespace rstracker.Core
{
    public class DataHandler
    {

        /* LoadAllPlayers will initialize all the stored player data upon launch of the application */

        public static void LoadAllPlayers()
        {
            string _dataPath = Constants.DATA_PATH;
            if (!Directory.Exists(_dataPath))
                Directory.CreateDirectory(_dataPath);

            var files = Directory.GetFiles(_dataPath, "*.json");
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var player = JsonConvert.DeserializeObject<PlayerModel>(json);
                if (player is null)
                    continue;
                Constants.PLAYERS.Add(player);
            }
            Utils.PrintLine($"Initialized {Constants.PLAYERS.Count} stored player data files.");
        }

        /* LoadPlayer initializes a specific player file from the username parameter */

        public static void LoadPlayer(string username)
        {
            string dataPath = Constants.DATA_PATH + Utils.Capitalize(username) + ".json";
            if (string.IsNullOrEmpty(dataPath) || !File.Exists(dataPath))
                throw new ArgumentNullException("Player not found.");

            var json = File.ReadAllText(dataPath);
            var player = JsonConvert.DeserializeObject<PlayerModel>(json) ?? throw new NullReferenceException("Player not found.");
            Constants.PLAYERS.Add(player);
        }

        /* Save creates or updates the player data */

        public static void Save(PlayerModel player)
        {
            if (player is null)
                throw new ArgumentNullException("Player could not be saved. Please try again");

            var json = JsonConvert.SerializeObject(player, Formatting.Indented);
            File.WriteAllTextAsync(Constants.DATA_PATH + player.Username + ".json", json);
        }

        /* GetPlayer will return a player in the stored memory or create a new instance of the player model */

        public static PlayerModel GetPlayer(string username)
        {
            foreach (var player in Constants.PLAYERS)
                if (player.Username == Utils.Capitalize(username))
                    return player;
            return new PlayerModel(username);
        }

    }
}
