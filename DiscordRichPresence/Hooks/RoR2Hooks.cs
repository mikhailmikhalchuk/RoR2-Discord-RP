using RoR2;
using System;
using DiscordRichPresence.Utils;
using static DiscordRichPresence.DiscordRichPresencePlugin;

namespace DiscordRichPresence.Hooks
{
    public static class RoR2Hooks
    {
        public static void AddHooks()
        {
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            CharacterBody.onBodyDestroyGlobal += CharacterBody_onBodyDestroyGlobal;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
            On.RoR2.TeleporterInteraction.FixedUpdate += TeleporterInteraction_FixedUpdate;
            On.RoR2.EscapeSequenceController.SetCountdownTime += EscapeSequenceController_SetCountdownTime;
            On.RoR2.InfiniteTowerRun.BeginNextWave += InfiniteTowerRun_BeginNextWave;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter += BaseMainMenuScreen_OnEnter;
            On.RoR2.Run.OnClientGameOver += Run_OnClientGameOver;
            On.RoR2.MoonBatteryMissionController.OnBatteryCharged += MoonBatteryMissionController_OnBatteryCharged;
        }

        private static void MoonBatteryMissionController_OnBatteryCharged(On.RoR2.MoonBatteryMissionController.orig_OnBatteryCharged orig, RoR2.MoonBatteryMissionController self, HoldoutZoneController holdoutzone)
        {
            orig(self, holdoutzone);
            MoonPillarsLeft = self.numRequiredBatteries;
            MoonPillars = self.numChargedBatteries;
            
            var richPresence = RichPresence;
            var activityManager = Client.GetActivityManager();
            activityManager.UpdateActivity(richPresence, (result =>
            {
                //LoggerEXT.LogInfo("activity updated, " + result);
            }));
            PresenceUtils.SetStagePresence(Client, richPresence, CurrentScene, Run.instance);
        }

        private static void Stage_onStageStartGlobal(Stage obj)
        {
            CharacterBody localBody = LocalUserManager.GetFirstLocalUser()?.cachedMasterController?.master?.GetBody(); // Don't know what exactly throws a null ref here so we'll just go all in on null checks
            if (localBody == null)
            {
                return;
            }

            var survivorname = InfoTextUtils.GetCharacterInternalName(localBody.baseNameToken);
            if (survivorname == "unknown") //fallback
            {
                if (InfoTextUtils.CharactersWithAssets.Contains(localBody.GetDisplayName().ToLower().Replace(" ", "")))
                {
                    survivorname = InfoTextUtils.GetCharacterInternalName(localBody.GetDisplayName().ToLower().Replace(" ", ""));
                }
                else
                {
                    //basically the easiest way to grab all the survivor icons is through the survivor catalog, then pull the names/icons through the survivor defs
                    // butttttttttt,, they can be mismatched from the localbody name that discord rpc uses (ss2 looking at u ,..,., 
                    //so as a last ditch we try grabbing it from the survivor def in case theres some mismatches in the repo (can happen when doing alot of them in batch
                    var survdefname = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.GetSurvivorIndexFromBodyIndex(localBody.bodyIndex))?.displayNameToken;
                    if (survdefname != null && InfoTextUtils.CharactersWithAssets.Contains(survdefname))
                    {
                        survivorname = InfoTextUtils.GetCharacterInternalName(survdefname);
                    }
                }
            } 
            //LoggerEXT.LogInfo($"nametoken :{localBody.baseNameToken} !!! found {survivorname} ,.."); //!!!USE THIS!!!
            
            var richPresence = RichPresence;
            richPresence.Assets.SmallImage = $"https://raw.githubusercontent.com/mikhailmikhalchuk/RoR2-Discord-RP/refs/heads/master/Assets/Characters/{survivorname}.png";
            richPresence.Assets.SmallText = localBody.GetDisplayName();
            var activityManager = Client.GetActivityManager();
            activityManager.UpdateActivity(richPresence, (result =>
            {
                //LoggerEXT.LogInfo("activity updated, " + result);
            }));
            //LoggerEXT.LogInfo(richPresence.Assets.SmallImage);
            PresenceUtils.SetStagePresence(Client, richPresence, CurrentScene, Run.instance);
        }

        public static void RemoveHooks()
        {
            CharacterBody.onBodyStartGlobal -= CharacterBody_onBodyStartGlobal;
            CharacterBody.onBodyDestroyGlobal -= CharacterBody_onBodyDestroyGlobal;
            Stage.onStageStartGlobal -= Stage_onStageStartGlobal;
            On.RoR2.TeleporterInteraction.FixedUpdate -= TeleporterInteraction_FixedUpdate;
            On.RoR2.EscapeSequenceController.SetCountdownTime -= EscapeSequenceController_SetCountdownTime;
            On.RoR2.InfiniteTowerRun.BeginNextWave -= InfiniteTowerRun_BeginNextWave;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter -= BaseMainMenuScreen_OnEnter;
            On.RoR2.Run.OnClientGameOver += Run_OnClientGameOver;
            On.RoR2.MoonBatteryMissionController.OnBatteryCharged += MoonBatteryMissionController_OnBatteryCharged;
        }

        private static void CharacterBody_onBodyStartGlobal(CharacterBody obj)
        {
            if (obj.isChampion)
            {
                CurrentBoss = obj.GetDisplayName();
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }
        }

        private static void CharacterBody_onBodyDestroyGlobal(CharacterBody obj)
        {
            if (obj.isChampion && Run.instance != null)
            {
                CurrentBoss = "";
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }
        }

        // We use this method because it provides a robust update system that updates only when we need it to; that is, when the teleporter is active and charging
        // Additionally, comparing with CurrentChargeLevel prevents unnecessary presence updates (which would lead to ratelimiting)
        private static void TeleporterInteraction_FixedUpdate(On.RoR2.TeleporterInteraction.orig_FixedUpdate orig, TeleporterInteraction self)
        {
            if (Math.Abs(Math.Round(self.chargeFraction, 2) - CurrentChargeLevel) > 0.005 && PluginConfig.TeleporterStatusEntry.Value == PluginConfig.TeleporterStatus.Charge && !RichPresence.State.Contains("Defeat!"))
            {
                CurrentChargeLevel = (float)Math.Round(self.chargeFraction, 2);
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }

            orig(self);
        }

        private static void EscapeSequenceController_SetCountdownTime(On.RoR2.EscapeSequenceController.orig_SetCountdownTime orig, EscapeSequenceController self, double secondsRemaining)
        {
            MoonCountdownTimer = (float)secondsRemaining + 1;
            PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);

            orig(self, secondsRemaining);
        }

        //Simulacrum
        private static void InfiniteTowerRun_BeginNextWave(On.RoR2.InfiniteTowerRun.orig_BeginNextWave orig, InfiniteTowerRun self)
        {
            PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, self);

            orig(self);
        }

        private static void BaseMainMenuScreen_OnEnter(On.RoR2.UI.MainMenu.BaseMainMenuScreen.orig_OnEnter orig, RoR2.UI.MainMenu.BaseMainMenuScreen self, RoR2.UI.MainMenu.MainMenuController mainMenuController)
        {
            if (Facepunch.Steamworks.Client.Instance.Lobby.IsValid) // Messy if-else, but the goal is that when exiting a multiplayer game to the menu, it will display the lobby presence instead of the main menu presence
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, Facepunch.Steamworks.Client.Instance);
            }
            else if (IsInEOSLobby)
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, EOSLobbyManager.GetFromPlatformSystems());
            }
            else
            {
                PresenceUtils.SetMainMenuPresence(Client, RichPresence);
            }

            orig(self, mainMenuController);
        }

        private static void Run_OnClientGameOver(On.RoR2.Run.orig_OnClientGameOver orig, Run self, RunReport runReport)
        {
            orig(self, runReport);
            if (Run.instance != null && CurrentScene != null)
            {
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance, true);
            }
            
            var richPresence = RichPresence;
            
            TimeSpan time = TimeSpan.FromSeconds((long)self.GetRunStopwatch());

            if ((long)self.GetRunStopwatch() > 60 * 60) // is it uhh longer then an hour 
            {
                richPresence.State = "Defeat! " +  time.ToString(@"hh\:mm\:ss") + " - " + richPresence.State;
            }
            else
            {
                richPresence.State = "Defeat! " +  time.ToString(@"mm\:ss") + " - " + richPresence.State;
            }
            
            var activityManager = Client.ActivityManagerInstance;
            
            activityManager.UpdateActivity(richPresence, (result =>
            {
                //LoggerEXT.LogInfo("activity updated, " + result);
            }));
            
            RichPresence = richPresence;
        }
    }
}