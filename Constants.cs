namespace rstracker
{
    public class Constants
    {

        /* 
         * GLOBAL_TRACKING_DELAY will store the time in milliseconds that a player can be tracked.
         *
         * The current delay is 60 seconds. This is to prevent the backend from being overloaded with requests.
         */

        public static long GLOBAL_TRACKING_DELAY = 60 * 1000;

    }
}
