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

        public static readonly List<Player> PLAYERS = new List<Player>();

        /* 
         * GLOBAL_TRACKING_DELAY will store the time in milliseconds that a player can be tracked.
         *
         * The current delay is 60 seconds. This is to prevent the backend from being overloaded with requests.
         */

        public static readonly long GLOBAL_TRACKING_DELAY = 60 * 1000;

    }
}
