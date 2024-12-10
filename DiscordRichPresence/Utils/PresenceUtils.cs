using Discord;
using RoR2;
using System;
using UnityEngine;
using static DiscordRichPresence.DiscordRichPresencePlugin;

namespace DiscordRichPresence.Utils
{
    public static class PresenceUtils
    {
        public static void SetStagePresence(Discord.Discord client, Activity richPresence, SceneDef scene, Run run, bool isPaused = false) // Don't like this ... needs to be decluttered
        {
            if (Run.instance == null)
            {
                LoggerEXT.LogError("Run instance is null. Check for its null status before passing it as a parameter. Stack trace follows:");
            }
            if (scene == null)
            {
                LoggerEXT.LogError("Scene is null. Check for its null status before passing it as a parameter. Stack trace follows:");
            }
            
            //LoggerEXT.LogInfo("baseSceneName: " + scene.baseSceneName); // uhhh yeah 
            
            /*
             something like this could be used to detect if the stage has a valid image without having to update the dll (and survivors too(
             //https://www.geeksforgeeks.org/how-to-read-data-from-website-using-c-sharp/
             using (var client = new HttpClient()) 
               { 
                   var response = await client.GetAsync("https://raw.githubusercontent.com/gamrtiem/RoR2-Discord-RP/refs/heads/master/Assets/" + scene.baseSceneName + ".png"); 

                   if (response.IsSuccessStatusCode) 
                   { 
                       var xml = await response.Content.ReadAsStringAsync(); 
  
                       // We can then use the LINQ to XML API to query the XML 
                       var doc = XDocument.Parse(xml); 
  
                       // Let's query the XML to get all of the <title> elements 
                       var images = from el in doc.Descendants("image") 
                                    select el.Value; 
                       
                       
                       // And finally, we'll print out the titles 
                       foreach (var images in images) 
                       { 
                           Console.WriteLine(images); 
                       } 
                   } 
               } 
             */
            if (InfoTextUtils.StagesWithAssets.Contains(scene.baseSceneName))
            {
                richPresence.Assets.LargeImage =
                    "https://raw.githubusercontent.com/mikhailmikhalchuk/RoR2-Discord-RP/refs/heads/master/Assets/" +
                    scene.baseSceneName + ".png";
            }
            else
            {
                richPresence.Assets.LargeImage =
                    "https://raw.githubusercontent.com/mikhailmikhalchuk/RoR2-Discord-RP/refs/heads/master/Assets/riskofrain2.png";
            }
            
            richPresence.Assets.LargeText = "DiscordRichPresence v" + Instance.Info.Metadata.Version;

            richPresence.State = string.Format("Stage {0} - {1}", run.stageClearCount + 1, Language.GetString(scene.nameToken));
            if (run is InfiniteTowerRun infRun && infRun.waveIndex > 0)
            {
                richPresence.State = string.Format("Wave {0} - {1}", infRun.waveIndex, Language.GetString(scene.nameToken));
            }

            string currentDifficultyString = Language.GetString(DifficultyCatalog.GetDifficultyDef(run.selectedDifficulty).nameToken);
            richPresence.Timestamps = new ActivityTimestamps(); // Clear timestamps
            richPresence.Secrets = new ActivitySecrets(); // Clear lobby join

            if (scene.baseSceneName == "outro")
            {
                MoonCountdownTimer = 0;
                richPresence.Assets.LargeImage = "moon2";
                richPresence.Details = "Credits";
                richPresence.State = string.Format("Stage {0} - {1}", run.stageClearCount + 1, Language.GetString(scene.nameToken));
            }
            else if (MoonCountdownTimer > 0)
            {
                richPresence.Details = "Escaping! | " + currentDifficultyString;
                if (!isPaused)
                {
                    richPresence.Timestamps.End = DateTimeOffset.Now.ToUnixTimeSeconds() + (long)MoonCountdownTimer;
                }
            }
            else
            {
                richPresence.Details = currentDifficultyString;
                if (PluginConfig.TeleporterStatusEntry.Value == PluginConfig.TeleporterStatus.Boss && CurrentBoss != "")
                {
                    richPresence.Details = "Fighting " + CurrentBoss + " | " + currentDifficultyString;
                }
                else if (PluginConfig.TeleporterStatusEntry.Value == PluginConfig.TeleporterStatus.Charge && CurrentChargeLevel > 0 && !Mathf.Approximately(CurrentChargeLevel, 1))
                {
                    richPresence.Details = "Charging teleporter (" + CurrentChargeLevel * 100 + "%) | " + currentDifficultyString;
                }

                if ((MoonPillars > 0 | MoonPillarsLeft > 0) && !Mathf.Approximately(MoonPillars, MoonPillarsLeft)) //idk rider wanted it like this and not moonpillars != moonpillarsleft because floating point numbers idk 
                {
                    richPresence.Details = "Charging pillars " + MoonPillars + "/" + MoonPillarsLeft + " | " + currentDifficultyString;
                }

                if (scene.sceneType == SceneType.Stage && !isPaused)
                {
                    richPresence.Timestamps.Start = DateTimeOffset.Now.ToUnixTimeSeconds() - (long)run.GetRunStopwatch();
                }
            }

            RichPresence = richPresence;
            var activityManager = client.ActivityManagerInstance;
            activityManager.UpdateActivity(richPresence, (result =>
            {
                LoggerEXT.LogInfo("activity updated, " + result);
            }));
        }

        public static void SetMainMenuPresence(Discord.Discord client, Activity richPresence, string details = "")
        {
            richPresence.Assets = new ActivityAssets()
            {
                LargeImage = "riskofrain2",
                LargeText = "DiscordRichPresence v" + Instance.Info.Metadata.Version
            };

            richPresence.Details = PluginConfig.MainMenuIdleMessageEntry.Value;
            if (details != "")
            {
                richPresence.Details = details;
            }

            richPresence.Timestamps = new ActivityTimestamps(); // Clear timestamps

            richPresence.State = "In Menu";
            richPresence.Secrets = new ActivitySecrets();
            richPresence.Party = new ActivityParty(); // Clear secrets and party

            RichPresence = richPresence;
            var activityManager = client.ActivityManagerInstance;
            activityManager.UpdateActivity(richPresence, (result =>
            {
                LoggerEXT.LogInfo("activity updated, " + result);
            }));
        }

        public static void SetLobbyPresence(Discord.Discord client, Activity richPresence, Facepunch.Steamworks.Client faceClient, bool justParty = false, string details = "")
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

            richPresence.Assets = new ActivityAssets()
            {
                LargeImage = "riskofrain2",
                LargeText = "DiscordRichPresence v" + Instance.Info.Metadata.Version
            };
            richPresence.Timestamps = new ActivityTimestamps(); // Clear timestamps

            Party:
            richPresence = UpdateParty(richPresence, faceClient);

            RichPresence = richPresence;
            var activityManager = client.ActivityManagerInstance;
            activityManager.UpdateActivity(richPresence, (result =>
            {
                LoggerEXT.LogInfo("activity updated, " + result);
            }));
        }

        public static void SetLobbyPresence(Discord.Discord client, Activity richPresence, EOSLobbyManager lobbyManager, bool justParty = false, string details = "")
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

            richPresence.Assets = new ActivityAssets()
            {
                LargeImage = "riskofrain2",
                LargeText = "DiscordRichPresence v" + Instance.Info.Metadata.Version
            };
            richPresence.Timestamps = new ActivityTimestamps(); // Clear timestamps

            Party:
            richPresence = UpdateParty(richPresence, lobbyManager);

            RichPresence = richPresence;
            var activityManager = client.ActivityManagerInstance;
            activityManager.UpdateActivity(richPresence, (result =>
            {
                LoggerEXT.LogInfo("activity updated, " + result);
            }));
        }

        public static Activity UpdateParty(Activity richPresence, Facepunch.Steamworks.Client faceClient, bool includeJoinButton = true)
        {
            richPresence.Party.Id = faceClient.Username;
            richPresence.Party.Size.CurrentSize = faceClient.Lobby.NumMembers;
            richPresence.Party.Size.MaxSize = faceClient.Lobby.MaxMembers;

            richPresence.Secrets = new ActivitySecrets();
            if (PluginConfig.AllowJoiningEntry.Value && includeJoinButton)
            {
                richPresence.Secrets.Join = faceClient.Lobby.CurrentLobby.ToString();
            }

            return richPresence;
        }

        public static Activity UpdateParty(Activity richPresence, EOSLobbyManager lobbyManager, bool includeJoinButton = true)
        {
            richPresence.Party.Id = lobbyManager.CurrentLobbyId;
            richPresence.Party.Size.CurrentSize = lobbyManager.newestLobbyData.totalMaxPlayers;
            richPresence.Party.Size.MaxSize = lobbyManager.newestLobbyData.totalPlayerCount;

            richPresence.Secrets = new ActivitySecrets();
            if (PluginConfig.AllowJoiningEntry.Value && includeJoinButton)
            {
                richPresence.Secrets.Join = lobbyManager.GetLobbyMembers()[0].ToString();
            }

            return richPresence;
        }
    }
}