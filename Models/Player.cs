﻿using rstracker.Enums;
using rstracker.Utility;

namespace rstracker.Models
{
    public class Player
    {

        /* Id will be the unique identifier of the player. This is generated so it is easier to track the player, even if they were to change usernames. */

        public string Id = Guid.NewGuid().ToString();

        /* LastResetDay will store the day where the player was last reset. This variable is used to track the player's daily experience gained */

        public DateTime LastResetDay { get; set; }

        /* LastUpdate will store the current time in milliseconds. This is to keep track of when the player was last updated and to prevent backend processing. */

        public long LastUpdate { get; set; }

        /* Username will store the player's username. This is used to display the player's username on the frontend. */

        public string Username { get; set; }

        /* SearchCount is a metric that will store the amount of times that the player has been searched for. */

        public int SearchCount { get; set; }

        /* SkillData will store the player's skill data. */

        public List<string> SkillData { get; set; }

        /* LastSkillDataUpdate will store the previous skill data. This is used to check for any XP or levels gained. */

        public List<string> LastSkillDataUpdate { get; set; }

        public Player(string username)
        {
            Username = Utils.Capitalize(username);
            SkillData = new List<string>();
            LastSkillDataUpdate = new List<string>();
            LastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + Constants.GLOBAL_TRACKING_DELAY;
            LastResetDay = DateTime.Today;
        }

        /* GetRank returns the Rank of the player */

        public int GetRank()
        {
            return int.Parse(SkillData[0].Split(",")[0]);
        }

        /* GetSkillLevel returns the level of the skill */

        public int GetSkillLevel(Skill skill)
        {
            return int.Parse(SkillData[(int)skill].Split(",")[1]);
        }

        /* GetSkillExperience returns the XP of the skill */

        public long GetSkillExperience(List<string> skillData, Skill skill)
        {
            return long.Parse(skillData[(int)skill].Split(',')[2]);
        }

        /* GetCombatLevel calculates the combat level of the player with the rules that applies of RuneScape 3 */

        public int GetCombatLevel()
        {
            int attackLevel = GetSkillLevel(Skill.ATTACK);
            int strengthLevel = GetSkillLevel(Skill.STRENGTH);
            int defenceLevel = GetSkillLevel(Skill.DEFENCE);
            int hitpointsLevel = GetSkillLevel(Skill.HITPOINTS);
            int prayerLevel = GetSkillLevel(Skill.PRAYER);
            int rangedLevel = GetSkillLevel(Skill.RANGED);
            int magicLevel = GetSkillLevel(Skill.MAGIC);
            int summoningLevel = GetSkillLevel(Skill.SUMMONING);
            int baseCombat = Math.Max(Math.Max(attackLevel + strengthLevel, magicLevel * 2), rangedLevel * 2);
            int combatLevel = (int)Math.Floor((double)(baseCombat * 1.3 + defenceLevel + hitpointsLevel + (prayerLevel / 2) + (summoningLevel / 2)) / 4);
            return combatLevel;
        }

        /* GetXPDifference returns the difference of XP gained. 
         *
         * If the player only has been searched upon one time (the initial search),
         * then there is no need to perform the calculations, as we do not have,
         * any data stored for this player yet.
         *
         */

        public long GetXPDifference(Skill skill)
        {
            if (SearchCount == 1)
                return 0;
            long oldXp = GetSkillExperience(LastSkillDataUpdate, skill);
            long newXp = GetSkillExperience(SkillData, skill);
            return newXp - oldXp;
        }

    }
}
