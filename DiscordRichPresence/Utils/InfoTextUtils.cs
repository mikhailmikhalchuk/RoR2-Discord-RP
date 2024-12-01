using System;
using System.Collections.Generic;

namespace DiscordRichPresence.Utils
{
    public static class InfoTextUtils
    {
        public static List<string> CharactersWithAssets = new List<string>()
        {
            "Acrid",
            "Artificer",
            "Bandit",
            "Captain",
            "Commando",
            "Engineer",
            "Heretic",
            "Huntress",
            "Loader",
            "MUL-T",
            "Mercenary",
            "REX",
            "Railgunner",
            //"Void Fiend",
            "False Son",
            "Seeker",
            "Chef",
            "CHEF",
            "Enforcer",
            "Miner",
            "Paladin",
            "HAN-D",
            "Sniper",
            "Bomber",
            "Nemesis Enforcer",
            "Nemesis Commando",
            "Nemesis Mercenary",
            "Chirr",
            "Executioner",
            "An Arbiter",
            "Red Mist",
            "Rocket",
            "Dancer",
            "Pilot",
            "Johnny",
            "Custodian",
            "Sonic",
            "Robomando",
            "Deputy",
            "Ranger",
            "Rifter",
            "Cadet",
            "Celestial War Tank",
            "Chrono Legionnaire BETA",
            "Chrono Legionnaire",
            "Cyborg",
            "Desolator",
            "Driver",
            "Interrogator",
            "Match Maker",
            "Mortician",
            "Nucleator",
            "Pathfinder",
            "Pyro",
            "Ravager",
            "Scout",
            "Seamstress",
            "Sorceress",
            "Spy",
            "Submariner",
            "Tesla Trooper",
            "Wanderer",
            "Cosmic Champion",
            "Belmont"
        };
        
        public static List<string> StagesWithAssets = new List<string>()
        {
            "agatevillage",
            "ancientloft",
            "arena",
            "artifactworld",
            "bazaar",
            "blackbeach",
            "blackbeach2",
            "BulwarksHaunt_GhostWave",
            "catacombs_DS1_Catacombs",
            "dampcavesimple",
            "drybasin",
            "FBLScene",
            "foggyswamp",
            "forgottenhaven",
            "frozenwall",
            "goldshores",
            "golemplains",
            "goolake",
            "habitat",
            "habitatfall",
            "helminthroost",
            "itancientloft",
            "itdampcave",
            "itfrozenwall",
            "itgolemplains",
            "itgoolake",
            "itmoon",
            "itskymeadow",
            "lakes",
            "lakesnight",
            "lemuriantemple",
            "limbo",
            "meridian",
            "moon2",
            "mysteryspace",
            "riskofrain2", // so there was a risk of rain ,., too ,.,,..,
            "shipgraveyard",
            "skymeadow",
            "slumberingsatellite",
            "sm64_bbf_SM64_BBF",
            "snowyforest",
            "sulfurpods",
            "village",
            "villagenight",
            "voidraid",
            "voidstage",
            "wispgraveyard"
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
            if (name == "「V??oid Fiend』")
            {
                return "voidfiend";
            }

            if (name == "CHEF") // gnome chef 
            {
                return "Chef";
            }

            if (name == "Chrono Legionnaire BETA")
            {
                return "chronolegionnaire";
            }
            if (CharactersWithAssets.Contains(name))
            {
                return CharactersWithAssets.Find(c => c == name).ToLower().Replace(" ", "");
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