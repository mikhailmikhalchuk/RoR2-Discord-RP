﻿using DiscordRPC;
using RoR2;
using System;
using static DiscordRichPresence.DiscordRichPresencePlugin;

namespace DiscordRichPresence.Utils
{
    public static class PresenceUtils
    {
        public static void SetStagePresence(DiscordRpcClient client, RichPresence richPresence, SceneDef scene, Run run, bool isPaused = false) // Don't like this ... needs to be decluttered
        {
            if (Run.instance == null)
            {
                LoggerEXT.LogError("Run instance is null. Check for its null status before passing it as a parameter. Stack trace follows:");
            }
            if (scene == null)
            {
                LoggerEXT.LogError("Scene is null. Check for its null status before passing it as a parameter. Stack trace follows:");
            }

            richPresence.Assets.LargeImageKey = scene.baseSceneName;
            richPresence.Assets.LargeImageText = "DiscordRichPresence v" + Instance.Info.Metadata.Version;

            richPresence.State = string.Format("Stage {0} - {1}", run.stageClearCount + 1, Language.GetString(scene.nameToken));
            if (run is InfiniteTowerRun infRun && infRun.waveIndex > 0)
            {
                richPresence.State = string.Format("Wave {0} - {1}", infRun.waveIndex, Language.GetString(scene.nameToken));
            }

            string currentDifficultyString = Language.GetString(DifficultyCatalog.GetDifficultyDef(run.selectedDifficulty).nameToken);
            richPresence.Timestamps = new Timestamps(); // Clear timestamps
            richPresence.Secrets = new Secrets(); // Clear lobby join

            if (scene.baseSceneName == "outro")
            {
                MoonCountdownTimer = 0;
                richPresence.Assets.LargeImageKey = "moon2";
                richPresence.Details = "Credits";
                richPresence.State = string.Format("Stage {0} - {1}", run.stageClearCount + 1, Language.GetString(scene.nameToken));
            }
            else if (MoonCountdownTimer > 0)
            {
                richPresence.Details = "Escaping! | " + currentDifficultyString;
                if (!isPaused)
                {
                    richPresence.Timestamps.EndUnixMilliseconds = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds() + (ulong)MoonCountdownTimer;
                }
            }
            else
            {
                richPresence.Details = currentDifficultyString;
                if (PluginConfig.TeleporterStatusEntry.Value == PluginConfig.TeleporterStatus.Boss && CurrentBoss != "")
                {
                    richPresence.Details = "Fighting " + CurrentBoss + " | " + currentDifficultyString;
                }
                else if (PluginConfig.TeleporterStatusEntry.Value == PluginConfig.TeleporterStatus.Charge && CurrentChargeLevel > 0)
                {
                    richPresence.Details = "Charging teleporter (" + CurrentChargeLevel * 100 + "%) | " + currentDifficultyString;
                }

                if (scene.sceneType == SceneType.Stage && !isPaused)
                {
                    richPresence.Timestamps.StartUnixMilliseconds = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds() - (ulong)run.GetRunStopwatch();
                }
            }

            DiscordRichPresencePlugin.RichPresence = richPresence;
            client.SetPresence(richPresence);
        }

        public static void SetMainMenuPresence(DiscordRpcClient client, RichPresence richPresence, string details = "")
        {
            richPresence.Assets = new Assets
            {
                LargeImageKey = "riskofrain2",
                LargeImageText = "DiscordRichPresence v" + Instance.Info.Metadata.Version
            };

            richPresence.Details = PluginConfig.MainMenuIdleMessageEntry.Value;
            if (details != "")
            {
                richPresence.Details = details;
            }

            richPresence.Timestamps = new Timestamps(); // Clear timestamps

            richPresence.State = "In Menu";
            richPresence.Secrets = new Secrets();
            richPresence.Party = new Party(); // Clear secrets and party

            DiscordRichPresencePlugin.RichPresence = richPresence;
            client.SetPresence(richPresence);
        }

        public static void SetLobbyPresence(DiscordRpcClient client, RichPresence richPresence, Facepunch.Steamworks.Client faceClient, bool justParty = false, string details = "")
        {
            if (justParty)
            {
                goto Party;
            }
            richPresence.State = "In Lobby";
            richPresence.Details = "Preparing";
            if (details != "")
            {
                richPresence.Details = details;
            }

            richPresence.Assets = new Assets
            {
                LargeImageKey = "riskofrain2",
                LargeImageText = "DiscordRichPresence v" + Instance.Info.Metadata.Version
            };
            richPresence.Timestamps = new Timestamps(); // Clear timestamps

            Party:
            richPresence = UpdateParty(richPresence, faceClient);

            DiscordRichPresencePlugin.RichPresence = richPresence;
            client.SetPresence(richPresence);
        }

        public static void SetLobbyPresence(DiscordRpcClient client, RichPresence richPresence, EOSLobbyManager lobbyManager, bool justParty = false, string details = "")
        {
            if (justParty)
            {
                goto Party;
            }
            richPresence.State = "In Lobby";
            richPresence.Details = "Preparing";
            if (details != "")
            {
                richPresence.Details = details;
            }

            richPresence.Assets = new Assets
            {
                LargeImageKey = "riskofrain2",
                LargeImageText = "DiscordRichPresence v" + Instance.Info.Metadata.Version
            };
            richPresence.Timestamps = new Timestamps(); // Clear timestamps

            Party:
            richPresence = UpdateParty(richPresence, lobbyManager);

            DiscordRichPresencePlugin.RichPresence = richPresence;
            client.SetPresence(richPresence);
        }

        public static RichPresence UpdateParty(RichPresence richPresence, Facepunch.Steamworks.Client faceClient, bool includeJoinButton = true)
        {
            richPresence.Party.ID = faceClient.Username;
            richPresence.Party.Max = faceClient.Lobby.MaxMembers;
            richPresence.Party.Size = faceClient.Lobby.NumMembers;

            richPresence.Secrets = new Secrets();
            if (PluginConfig.AllowJoiningEntry.Value && includeJoinButton)
            {
                richPresence.Secrets.JoinSecret = faceClient.Lobby.CurrentLobby.ToString();
            }

            return richPresence;
        }

        public static RichPresence UpdateParty(RichPresence richPresence, EOSLobbyManager lobbyManager, bool includeJoinButton = true)
        {
            richPresence.Party.ID = lobbyManager.CurrentLobbyId;
            richPresence.Party.Max = lobbyManager.newestLobbyData.totalMaxPlayers;
            richPresence.Party.Size = lobbyManager.newestLobbyData.totalPlayerCount; // GetLobbyMembers().Length

            richPresence.Secrets = new Secrets();
            if (PluginConfig.AllowJoiningEntry.Value && includeJoinButton)
            {
                richPresence.Secrets.JoinSecret = lobbyManager.GetLobbyMembers()[0].ToString();
            }

            return richPresence;
        }
    }
}