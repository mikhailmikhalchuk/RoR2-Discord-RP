﻿using System;
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
            "Enforcer",
            "Miner",
            "Paladin",
            "HAN-D",
            "Sniper",
            "Bomber",
            "Nemesis Enforcer",
            "Nemesis Commando",
            "Chirr",
            "Executioner",
            "An Arbiter",
            "Red Mist"
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