using BepInEx.Configuration;

namespace DiscordRichPresence
{
    public static class PluginConfig
    {
        public static ConfigEntry<bool> AllowJoiningEntry { get; set; }

        public static ConfigEntry<TeleporterStatus> TeleporterStatusEntry { get; set; }

        public static ConfigEntry<string> MainMenuIdleMessageEntry { get; set; }

        public enum TeleporterStatus : byte
        {
            Boss = 0,
            Charge = 1
        }
    }
}