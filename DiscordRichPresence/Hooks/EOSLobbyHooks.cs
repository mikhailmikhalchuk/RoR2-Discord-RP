using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
using RoR2;
using System;
using DiscordRichPresence.Utils;
using static DiscordRichPresence.DiscordRichPresencePlugin;

namespace DiscordRichPresence.Hooks
{
    /// <summary>
    /// Handles Epic Online Services (EOS) lobby connections and methods.
    /// </summary>
    public static class EOSLobbyHooks
    {
        public static void AddHooks()
        {
            try
            {
                On.RoR2.EOSLobbyManager.OnLobbyCreated += EOSLobbyManager_OnLobbyCreated;
                On.RoR2.EOSLobbyManager.OnLobbyJoined += EOSLobbyManager_OnLobbyJoined;
                On.RoR2.EOSLobbyManager.OnLobbyChanged += EOSLobbyManager_OnLobbyChanged;
                On.RoR2.EOSLobbyManager.LeaveLobby += EOSLobbyManager_LeaveLobby;
            }
            catch (BadImageFormatException)
            {
                LoggerEXT.LogError("Couldn't hook EOS methods");
            }
        }

        public static void RemoveHooks()
        {
            try
            {
                On.RoR2.EOSLobbyManager.OnLobbyCreated -= EOSLobbyManager_OnLobbyCreated;
                On.RoR2.EOSLobbyManager.OnLobbyJoined -= EOSLobbyManager_OnLobbyJoined;
                On.RoR2.EOSLobbyManager.OnLobbyChanged -= EOSLobbyManager_OnLobbyChanged;
                On.RoR2.EOSLobbyManager.LeaveLobby -= EOSLobbyManager_LeaveLobby;
            }
            catch (BadImageFormatException)
            {
                LoggerEXT.LogError("Couldn't unhook EOS methods");
            }
        }

        private static void EOSLobbyManager_OnLobbyCreated(On.RoR2.EOSLobbyManager.orig_OnLobbyCreated orig, EOSLobbyManager self, ref CreateLobbyCallbackInfo data)
        {
            orig(self, ref data);

            if (data.ResultCode != Result.Success || self == null)
            {
                return;
            }

            LoggerEXT.LogInfo("Discord broadcasting new EOS lobby with ID " + self.CurrentLobbyId);

            PresenceUtils.SetLobbyPresence(Client, RichPresence, self);
        }

        private static void EOSLobbyManager_OnLobbyJoined(On.RoR2.EOSLobbyManager.orig_OnLobbyJoined orig, EOSLobbyManager self, ref JoinLobbyCallbackInfo data)
        {
            orig(self, ref data);

            if (data.ResultCode != Result.Success || self == null)
            {
                return;
            }

            LoggerEXT.LogInfo("Successfully joined EOS lobby");

            PresenceUtils.SetLobbyPresence(Client, RichPresence, self);
        }

        private static void EOSLobbyManager_OnLobbyChanged(On.RoR2.EOSLobbyManager.orig_OnLobbyChanged orig, EOSLobbyManager self)
        {
            orig(self);

            if (self == null || !self.isInLobby)
            {
                return;
            }

            LoggerEXT.LogInfo("Discord re-broadcasting EOS lobby");

            if (Run.instance == null)
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, self, RichPresence.Details == "Choosing Character");
            }
            else
            {
                RichPresence = PresenceUtils.UpdateParty(RichPresence, self, false);
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }
        }

        private static void EOSLobbyManager_LeaveLobby(On.RoR2.EOSLobbyManager.orig_LeaveLobby orig, EOSLobbyManager self)
        {
            orig(self);

            if (Client == null) //|| !Client.IsInitialized)
            {
                return;
            }

            PresenceUtils.SetMainMenuPresence(Client, RichPresence);
        }

        //Ensures that the game will not load hooks if the user isn't even signed into their Epic account
        public static void EOSLoginManager_CompleteConnectLogin(On.RoR2.EOSLoginManager.orig_CompleteConnectLogin orig, EOSLoginManager self, ProductUserId localUserId)
        {
            orig(self, localUserId);

            AddHooks();
            LoggerEXT.LogInfo(EOSLoginManager.loggedInUserID.ToString());
        }
    }
}