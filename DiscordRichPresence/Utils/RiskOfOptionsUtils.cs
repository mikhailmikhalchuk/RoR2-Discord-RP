using BepInEx.Bootstrap;
using BepInEx.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;

namespace DiscordRichPresence.Utils
{
    public static class RiskOfOptionsUtils
    {
        public static bool IsEnabled => Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        public static void AddIcon()
        {
            FileInfo iconFile = null;

            DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty);

            FileInfo[] files = directory.GetFiles("icon.png", SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
            {
                iconFile = files[0];
            }

            if (iconFile != null)
            {
                Texture2D icon = new Texture2D(256, 256);
                if (icon.LoadImage(File.ReadAllBytes(iconFile.FullName)))
                {
                    RiskOfOptions.ModSettingsManager.SetModIcon(Sprite.Create(icon, new Rect(0f, 0f, icon.width, icon.height), new Vector2(0.5f, 0.5f)));
                }
            }
        }

        public static void AddCheckBoxOption(ConfigEntry<bool> entry)
        {
            RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(entry));
        }

        public static void AddMultiOption<T>(ConfigEntry<T> entry) where T : Enum
        {
            RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.ChoiceOption(entry));
        }

        public static void AddTextInputOption(ConfigEntry<string> entry)
        {
            RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.StringInputFieldOption(entry));
        }

        public static void SetModDescription(string description)
        {
            RiskOfOptions.ModSettingsManager.SetModDescription(description);
        }
    }
}