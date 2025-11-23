using System;
using System.Collections.Generic;

namespace DiscordRichPresence.Utils
{
    public static class InfoTextUtils
    {
        public static List<string> CharactersWithAssets = new List<string>()
        {
            "anarbiter",
            "unknown",
            "cosmicchampion",
            "deputy",
            "johnny",
            "redmist",
            "BANDIT2_BODY_NAME",
            "BLAKE_Rifter_NAME",
            "BOG_MORRIS_BODY_NAME",
            "BOG_PATHFINDER_BODY_NAME",
            "BOMBER_NAME",
            "CAPTAIN_BODY_NAME",
            "CHEESEWITHHOLES_BASICTANK_BODY_NAME",
            "CHEF_BODY_NAME",
            "CLOUDBURST_WYATT_NAME",
            "COMMANDO_BODY_NAME",
            "CROCO_BODY_NAME",
            "DS_GAMING_SONIC_THE_HEDGEHOG_BODY_NAME",
            "ENFORCER_NAME",
            "ENGI_BODY_NAME",
            "FALSESON_BODY_NAME",
            "FROSTHEX_ASTERIA_NAME",
            "GNOMECHEF_NAME",
            "HABIBI_CHRONO_NAME",
            "HABIBI_DESOLATOR_BODY_NAME",
            "HABIBI_MATCHER_NAME",
            "HABIBI_TESLA_BODY_NAME",
            "HERETIC_BODY_NAME",
            "HUNTRESS_BODY_NAME",
            "KENKO_BANSHEE_NAME",
            "KENKO_CADET_NAME",
            "KENKO_INTERROGATOR_NAME",
            "KENKO_SCOUT_NAME",
            "KENKO_SEAMSTRESS_NAME",
            "KENKO_SUBMARINER_NAME",
            "KENKO_UNFORGIVEN_NAME",
            "LOADER_BODY_NAME",
            "MAGE_BODY_NAME",
            "MERC_BODY_NAME",
            "MINER_NAME",
            "MOFFEIN_HAND_BODY_NAME",
            "MOFFEIN_PILOT_BODY_NAME",
            "MOFFEIN_ROCKET_BODY_NAME",
            "NDP_DANCER_BODY_NAME",
            "NEMFORCER_NAME",
            "PALADIN_NAME",
            "RAILGUNNER_BODY_NAME",
            "RAT_ROBOMANDO_NAME",
            "RL_BLMERC_NAME",
            "RL_EGO_GRINDER_BODY_NAME",
            "RL_EGO_JUSTITIA_NAME",
            "RL_EGO_LAMENT_NAME",
            "RL_EGO_LAMP_NAME",
            "RL_EGO_MAGICBULLET_NAME",
            "RL_EGO_MIMICRY_NAME",
            "RL_SWEEPER_NAME",
            "ROB_BELMONT_BODY_NAME",
            "ROB_DANTE_BODY_NAME",
            "ROB_DRIVER_BODY_NAME",
            "ROB_RAVAGER_BODY_NAME",
            "SANDSWEPT_ELECTR_NAME",
            "SEEKER_BODY_NAME",
            "SNIPERCLASSIC_BODY_NAME",
            "SS2_CHIRR_BODY_NAME",
            "SS2_EXECUTIONER2_NAME",
            "SS2_NEMMANDO_NAME",
            "SS2_NEMESIS_MERCENARY_BODY_NAME",
            "SS2UCHIRR_NAME",
            "SS2UCYBORG_NAME",
            "SS2UEXECUTIONER_NAME",
            "SS2UNEMMANDO_NAME",
            "SS2UNUCLEATOR_NAME",
            "SS2UPYRO_NAME",
            "SS_RANGER_BODY_NAME",
            "TOOLBOT_BODY_NAME",
            "TREEBOT_BODY_NAME",
            "VOIDSURVIVOR_BODY_NAME",
            "DRIFTER_BODY_NAME",
            "DRONETECH_BODY_NAME"
        };
        
        public static List<string> StagesWithAssets = new List<string>()
        {
            //you can copy past this from a bash oneliner to get this             for file in *.png; do echo "\"${file%.png}\","; done
            "riskofrain2", //default
            "BulwarksHaunt_GhostWave", //currently deprecated so cant get <//3
            "forgottenhaven",
            "drybasin",
            "slumberingsatellite",
            "MAP_AGATE_VILLAGE_NAME", //vanilla
            "MAP_ANCIENTLOFT_TITLE",
            "MAP_ARENA_TITLE",
            "MAP_ARTIFACTWORLD_TITLE",
            "MAP_BAZAAR_TITLE",
            "MAP_BLACKBEACH_TITLE",
            "MAP_DAMPCAVE_TITLE",
            "MAP_FOGGYSWAMP_TITLE",
            "MAP_FROZENWALL_TITLE",
            "MAP_GOLDSHORES_TITLE",
            "MAP_GOLEMPLAINS_TITLE",
            "MAP_GOOLAKE_TITLE",
            "MAP_HABITATFALL_TITLE",
            "MAP_HABITAT_TITLE",
            "MAP_HELMINTHROOST_TITLE",
            "MAP_itancientloft_NAME",
            "MAP_itdampcave_NAME",
            "MAP_itfrozenwall_NAME",
            "MAP_itgolemplains_NAME",
            "MAP_itgoolake_NAME",
            "MAP_itmoon_NAME",
            "MAP_itskymeadow_NAME",
            "MAP_LAKESNIGHT_TITLE",
            "MAP_LAKES_TITLE",
            "MAP_LEMURIANTEMPLE_TITLE",
            "MAP_LIMBO_TITLE",
            "MAP_MERIDIAN_TITLE",
            "MAP_MOON_TITLE",
            "MAP_MYSTERYSPACE_TITLE",
            "MAP_ROOTJUNGLE_TITLE",
            "MAP_SHIPGRAVEYARD_TITLE",
            "MAP_SKYMEADOW_TITLE",
            "MAP_SNOWYFOREST_TITLE",
            "MAP_SULFURPOOLS_TITLE",
            "MAP_VILLAGENIGHT_TITLE",
            "MAP_VILLAGE_TITLE",
            "MAP_VOIDRAID_TITLE",
            "MAP_VOIDSTAGE_TITLE",
            "MAP_WISPGRAVEYARD_TITLE",
            "MAP_SOLUTIONALHAUNT_TITLE", // dlc3
            "MAP_SOLUSWEB_NAME",
            "MAP_REPURPOSEDCRATER_TITLE",
            "MAP_NEST_TITLE",
            "MAP_IRONALLUVIUM2_TITLE",
            "MAP_IRONALLUVIUM_TITLE",
            "MAP_CONDUITCANYON_TITLE",
            "MAP_COMPUTATIONALEXCHANGE_TITLE",
            "SNOWTIME_MAP_BLOODGULCH_0", //snowtime
            "SNOWTIME_MAP_CITY_0",
            "SNOWTIME_MAP_DEATHISLAND_0",
            "SNOWTIME_MAP_DHALO_0",
            "SNOWTIME_MAP_FLAT_0",
            "SNOWTIME_MAP_GPH_0",
            "SNOWTIME_MAP_HALO_0",
            "SNOWTIME_MAP_HC_0",
            "SNOWTIME_MAP_HIGHTOWER_0",
            "SNOWTIME_MAP_IF_0",
            "SNOWTIME_MAP_NMB_0",
            "SNOWTIME_MAP_SHRINE_0",
            "SNOWTIME_MAP_SW_0",
            "SNOWTIME_MAP_GMC_0",
            "FOGBOUND_SCENEDEF_NAME_TOKEN", //misc modded
            "CATACOMBS_MAP_DS1_CATACOMBS_NAME",
            "SM64_BBF_MAP_SM64_BBF_NAME",
        };
        public enum StyleTag : byte
        {
            Damage = 1,
            Healing = 2,
            Utility = 3,
            Health = 4,
            Stack = 5,
            Mono = 6,
            Death = 7,
            UserSetting = 8,
            Artifact = 9,
            Sub = 10,
            Event = 11,
            WorldEvent = 12,
            KeywordName = 13,
            Shrine = 14
        }

        public static string GetCharacterInternalName(string name)
        {
            if (CharactersWithAssets.Contains(name))
            {
                return CharactersWithAssets.Find(c => c == name);
            }
            return "unknown";
        }

        public static string FormatTextStyleTag(string content, StyleTag styleTag)
        {
            string tagString;
            if ((byte)styleTag >= 1 && (byte)styleTag <= 4)
            {
                tagString = "cIs" + styleTag.ToString();
            }
            else
            {
                tagString = "c" + styleTag.ToString();
            }
            return $"<style={tagString}>{content}</style>";
        }
    }
}