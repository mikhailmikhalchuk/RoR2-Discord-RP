using RoR2;
using DiscordRichPresence.Utils;
using static DiscordRichPresence.DiscordRichPresencePlugin;

namespace DiscordRichPresence.Hooks
{
    /// <summary>
    /// Handles Steamworks lobby connections and methods.
    /// </summary>
    public static class SteamworksLobbyHooks
    {
        public static void AddHooks()
        {
            On.RoR2.SteamworksLobbyManager.OnLobbyCreated += SteamworksLobbyManager_OnLobbyCreated;
            //On.RoR2.SteamworksLobbyManager.OnLobbyJoined += SteamworksLobbyManager_OnLobbyJoined; you cant hook onto this anymore; no idea why ! the actual function calls lobbychanged right after so it'll probably be fine :soycat:
            On.RoR2.SteamworksLobbyManager.OnLobbyChanged += SteamworksLobbyManager_OnLobbyChanged;
            On.RoR2.SteamworksLobbyManager.LeaveLobby += SteamworksLobbyManager_LeaveLobby;
        }

        public static void RemoveHooks()
        {
            On.RoR2.SteamworksLobbyManager.OnLobbyCreated -= SteamworksLobbyManager_OnLobbyCreated;
            //On.RoR2.SteamworksLobbyManager.OnLobbyJoined -= SteamworksLobbyManager_OnLobbyJoined;
            On.RoR2.SteamworksLobbyManager.OnLobbyChanged -= SteamworksLobbyManager_OnLobbyChanged;
            On.RoR2.SteamworksLobbyManager.LeaveLobby -= SteamworksLobbyManager_LeaveLobby;
        }

        private static void SteamworksLobbyManager_OnLobbyCreated(On.RoR2.SteamworksLobbyManager.orig_OnLobbyCreated orig, SteamworksLobbyManager self, bool success)
        {
            orig(self, success);

            if (!success || Facepunch.Steamworks.Client.Instance == null)
            {
                return;
            }

            LoggerEXT.LogInfo("Discord broadcasting new Steam lobby with ID " + Facepunch.Steamworks.Client.Instance.Lobby.CurrentLobby);

            PresenceUtils.SetLobbyPresence(Facepunch.Steamworks.Client.Instance);
        }

        /*
        private static void SteamworksLobbyManager_OnLobbyJoined(On.RoR2.SteamworksLobbyManager.orig_OnLobbyJoined orig, SteamworksLobbyManager self, bool success)
        {
            orig(self, success);

            if (!success || Facepunch.Steamworks.Client.Instance == null)
            {
                return;
            }

            LoggerEXT.LogInfo("Successfully joined Steam lobby");

            PresenceUtils.SetLobbyPresence(Facepunch.Steamworks.Client.Instance);
        }
        */
        
        private static void SteamworksLobbyManager_OnLobbyChanged(On.RoR2.SteamworksLobbyManager.orig_OnLobbyChanged orig, SteamworksLobbyManager self)
        {
            orig(self);

            if (!self.isInLobby || Facepunch.Steamworks.Client.Instance == null)
            {
                return;
            }

            LoggerEXT.LogInfo("Discord re-broadcasting Steam lobby");

            if (Run.instance == null)
            {
                PresenceUtils.SetLobbyPresence(Facepunch.Steamworks.Client.Instance, (RichPresence.Details == "Choosing Character"));
            }
            else
            {
                RichPresence = PresenceUtils.UpdateParty(RichPresence, Facepunch.Steamworks.Client.Instance, false);
                PresenceUtils.SetStagePresence(CurrentScene, Run.instance);
            }
        }

        private static void SteamworksLobbyManager_LeaveLobby(On.RoR2.SteamworksLobbyManager.orig_LeaveLobby orig, SteamworksLobbyManager self)
        {
            orig(self);

            if (Client == null) //|| !Client.IsInitialized)
            {
                return;
            }

            PresenceUtils.SetMainMenuPresence();
        }
    }
}