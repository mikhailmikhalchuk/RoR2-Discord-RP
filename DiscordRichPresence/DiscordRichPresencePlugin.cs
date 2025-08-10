using System;
using BepInEx;
using BepInEx.Logging;
using Discord;
using DiscordRichPresence.Hooks;
using DiscordRichPresence.Utils;
using RoR2;
using R2API.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

// Thanks to WhelanB (to which this repository originates from)
// and DarkKronicle (whose repository this is forked from)
// and icebro/bread (who fixed compatibility with SOTS)

namespace DiscordRichPresence
{
    [BepInPlugin("com.cuno.discord", "Discord Rich Presence", "1.3.4")]

    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]

    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]

    public class DiscordRichPresencePlugin : BaseUnityPlugin
    {
        internal static ManualLogSource LoggerEXT { get; private set; }

        public static Discord.Discord Client { get; set; }

        public static Activity RichPresence { get; set; }

        public static DiscordRichPresencePlugin Instance { get; private set; }

        public static SceneDef CurrentScene => SceneCatalog.GetSceneDefForCurrentScene();
        
        public static float CurrentChargeLevel { get; set; }
        
        public static float MoonPillars { get; set; }
        
        public static float MoonPillarsLeft { get; set; }

        public static float MoonCountdownTimer { get; set; }

        public static string CurrentBoss { get; set; }

        public static bool IsInEOSLobby => EOSLobbyManager.GetFromPlatformSystems() != null && EOSLobbyManager.GetFromPlatformSystems().isInLobby;

        public void ChangeActivity()
        {
            var activityManager = Client.GetActivityManager();
            RichPresence = new Activity
            {
                State = "Starting game...",
            };
            activityManager.UpdateActivity(RichPresence, (result =>
            {
                //LoggerEXT.LogInfo("activity updated, " + result);
            }));
        }

        private void Update()
        {
            if (Client != null)
            {
                Client.RunCallbacks();
            }
            else
            {
                LoggerEXT.LogInfo("discord is null");
            }
            
            //use this for debugging surv and stage nametokens 
            // if (Input.GetKeyDown(KeyCode.F2))
            // {
            //     foreach (var sceneDef in SceneCatalog.allStageSceneDefs)
            //     {
            //         LoggerEXT.LogDebug($"scene name: \"{sceneDef.baseSceneName}\", name token \"{sceneDef.nameToken}\"");
            //     }
            //     foreach (var survDef in SurvivorCatalog.allSurvivorDefs)
            //     {
            //         LoggerEXT.LogDebug($"surv name: \"{survDef.cachedName}\", name token \"{survDef.displayNameToken}\""); //these are sometimes wrong ?? looking at you ss2 ,,..,
            //     }
            // }
        }

        public void Awake()
        {
            Instance = this;
            LoggerEXT = Logger;
            Logger.LogInfo("Starting Discord Rich Presence...");
            
            Client = new Discord.Discord(992086428240580720, (ulong)CreateFlags.NoRequireDiscord);
            ChangeActivity();
            
            var activityManager = Client.GetActivityManager();
            Client.GetActivityManager();
            Activity richPresence = new Activity
            {
                State = "Starting game...",
                Assets = new ActivityAssets(),
                Secrets = new ActivitySecrets(),
                Timestamps = new ActivityTimestamps()
            };
            richPresence.Assets.LargeImage = "https://raw.githubusercontent.com/mikhailmikhalchuk/RoR2-Discord-RP/refs/heads/master/Assets/riskofrain2.png";
            richPresence.Assets.LargeText = "DiscordRichPresence v" + Instance.Info.Metadata.Version;
            RichPresence = richPresence;
            activityManager.UpdateActivity(RichPresence, (result =>
            {
                //LoggerEXT.LogInfo("activity updated, " + result);
            }));
            
            Logger.LogInfo("Discord Rich Presence has started...");
            
            
            PluginConfig.AllowJoiningEntry = Config.Bind("Options", "Allow Joining", true, "Controls whether or not other users should be allowed to ask to join your game.");
            PluginConfig.TeleporterStatusEntry = Config.Bind("Options", "Teleporter Status", PluginConfig.TeleporterStatus.None, "Controls whether the teleporter boss, teleporter charge status, or neither, should be shown alongside the current difficulty.");
            PluginConfig.MainMenuIdleMessageEntry = Config.Bind("Options", "Main Menu Idle Message", "", "Allows you to choose a message to be displayed when idling in the main menu.");

            if (RiskOfOptionsUtils.IsEnabled)
            {
                RiskOfOptionsUtils.AddIcon();
                RiskOfOptionsUtils.SetModDescription("Adds Discord Rich Presence functionality to Risk of Rain 2");
                RiskOfOptionsUtils.AddCheckBoxOption(PluginConfig.AllowJoiningEntry);
                RiskOfOptionsUtils.AddMultiOption(PluginConfig.TeleporterStatusEntry);
                RiskOfOptionsUtils.AddTextInputOption(PluginConfig.MainMenuIdleMessageEntry);
            }
        }

        private static void InitializeHooks()
        {
            //DiscordClientHooks.AddHooks(discord);
            PauseManagerHooks.AddHooks();
            SteamworksLobbyHooks.AddHooks();
            RoR2Hooks.AddHooks();

            //On.RoR2.EOSLoginManager.CompleteConnectLogin += EOSLobbyHooks.EOSLoginManager_CompleteConnectLogin;
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            Stage.onServerStageBegin += Stage_onServerStageBegin;
        }

        public static void Dispose()
        {
            //DiscordClientHooks.RemoveHooks(discord);
            PauseManagerHooks.RemoveHooks();
            SteamworksLobbyHooks.RemoveHooks();
            RoR2Hooks.RemoveHooks();

            if (EOSLoginManager.loggedInUserID.ToString() != string.Empty)
            {
                EOSLobbyHooks.RemoveHooks();
            }

            //On.RoR2.EOSLoginManager.CompleteConnectLogin -= EOSLobbyHooks.EOSLoginManager_CompleteConnectLogin;
            SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
            Stage.onServerStageBegin -= Stage_onServerStageBegin;

            Client.Dispose();
        }

        public void OnEnable()
        {
            InitializeHooks();
        }

        public void OnDisable()
        {
            Dispose();
        }

        private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if (Client == null)
            {
                return;
            }

            CurrentBoss = "";
            CurrentChargeLevel = 0;
            MoonPillars = 0;
            MoonPillarsLeft = 0;
            MoonCountdownTimer = 0;

            EOSLobbyManager lobbyManager = EOSLobbyManager.GetFromPlatformSystems();

            if (arg1.name == "title" && Facepunch.Steamworks.Client.Instance.Lobby.IsValid)
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, Facepunch.Steamworks.Client.Instance);
            }
            else if (arg1.name == "title" && IsInEOSLobby)
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, lobbyManager);
            }
            if (arg1.name == "lobby" && !Facepunch.Steamworks.Client.Instance.Lobby.IsValid && !IsInEOSLobby)
            {
                PresenceUtils.SetMainMenuPresence(Client, RichPresence, "Choosing Character");
            }
            else if (arg1.name == "lobby" && Facepunch.Steamworks.Client.Instance.Lobby.IsValid)
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, Facepunch.Steamworks.Client.Instance, false, "Choosing Character");
            }
            else if (arg1.name == "lobby" && IsInEOSLobby)
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, lobbyManager, false, "Choosing Character");
            }
            if (arg1.name == "logbook")
            {
                PresenceUtils.SetMainMenuPresence(Client, RichPresence, "Reading Logbook");
            }
            else if (Run.instance != null && CurrentScene != null && (Facepunch.Steamworks.Client.Instance.Lobby.IsValid || IsInEOSLobby))
            {
                LoggerEXT.LogInfo("Scene Manager Active Scene Changed Called With Value: " + (Run.instance.stageClearCount + 1));
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }
        }

        private static void Stage_onServerStageBegin(Stage obj)
        {
            CurrentChargeLevel = 0;
            MoonPillars = 0; // resetting it here for the sillies who have mods that let you loop past mithrix ,.,
            MoonPillarsLeft = 0;
            if (CurrentScene != null && Run.instance != null) // Test: Stage 1 --> 2 on 2 player MP
            {
                //LoggerEXT.LogInfo("Stage On Server Stage Begin Called With Value: " + obj.sceneDef.stageOrder);
                //LoggerEXT.LogInfo("Stage On Server Stage Begin Called With Run Instance Value: " + (Run.instance.stageClearCount + 1));
                
                
                
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }
        }
    }
}
