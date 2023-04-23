using rstracker.Enums;

namespace rstracker.Models
{
    public class GameModeModel
    {

        public GameMode GameMode = GameMode.NONE;

        public bool IsDead { get; set; }

        public string DeathDate = string.Empty;

        public string DeathLocation = string.Empty;

        public string DeathReason = string.Empty;

        /* GetGamdeMode will return the string location to the sprite, so that the player's gamemode can be displayed in the frontend */

        public string GetGameMode()
        {
            if (GameMode == GameMode.REGULAR)
                return string.Empty;
            return (GameMode == GameMode.HARDCORE_IRONMAN ? "HC ironman" : "ironman") + ".png";
        }

        /* GameModeTitle will return the game mode title to be displayed in the frontend */

        public string GetGameModeTitle()
        {
            if (GameMode == GameMode.REGULAR)
                return string.Empty;
            return GameMode == GameMode.HARDCORE_IRONMAN ? "Hardcore Ironman" : "Ironman";
        }

    }
}
