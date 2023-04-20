using rstracker.Models;

namespace rstracker
{
    public class Constants
    {

        /*
         * 
         * DATA_PATH is the path to the data folder. This solution is temporarily and will store player data locally until a database is implemented.
         * 
         * PLAYERS is also a temporarily solution and will store player data that is initialized until a database is implemented.
         * 
         */

        public static readonly string DATA_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "rstracker\\");

        public static readonly string ERROR_LOG_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "rstracker\\errorlogs\\");

        public static readonly List<Player> PLAYERS = new List<Player>();

        /* 
         * GLOBAL_TRACKING_DELAY will store the time in milliseconds that a player can be tracked.
         *
         * The current delay is 60 seconds. This is to prevent the backend from being overloaded with requests.
         */

        public static readonly long GLOBAL_TRACKING_DELAY = 60 * 1000;

        /**
         * 
         * API ENDPOINTS
         * 
         * Highscore (lite) endpoint
         * 
         * */

        public static string GetHighscoreEndPoint(string username)
        {
            return $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={username}";
        }

        /**
         * 
         * API ENDPOINTS
         * 
         * Clan data endpoint
         * 
         * */

        public static string GetClanDataEndPoint(string username)
        {
            return $"https://secure.runescape.com/m=website-data/playerDetails.ws?names=%5B%22{username}%22%5D&callback=jQuery000000000000000_0000000000&_=0";
        }

    }
}
