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

        public static readonly List<PlayerModel> PLAYERS = new List<PlayerModel>();

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

        /**
         * 
         * API ENDPOINTS
         * 
         * Hardcore ironman endpoint
         * 
         * This will be used to determine if the user is a hardcore ironman
         * 
         * */

        public static string GetHardcoreIronmanEndPoint(string username)
        {
            return $"https://secure.runescape.com/m=hiscore_hardcore_ironman/index_lite.ws?player={username}";
        }

        /**
        *
        * URL ENDPOINT
        * 
        * Hardcore ironman highscore
        * 
        * This will be used to determine the death cause and if they are dead as hardcore ironman.
        *
        **/

        public static string GetHardcoreIronmanHighscore(string username)
        {
            return $"https://secure.runescape.com/m=hiscore_hardcore_ironman/ranking?table=0&category_type=0&time_filter=0&date=1682249016673&user={username}";
        }

        /**
         * 
         * API ENDPOINTS
         * 
         * Ironman endpoint
         * 
         * This will be used to determine if the user is a normal ironman
         * 
         * */

        public static string GetIronmanEndPoint(string username)
        {
            return $"https://secure.runescape.com/m=hiscore_ironman/index_lite.ws?player={username}";
        }

    }
}
