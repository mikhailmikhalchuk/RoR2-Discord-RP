using RoR2;
using DiscordRichPresence.Utils;
using static DiscordRichPresence.DiscordRichPresencePlugin;

namespace DiscordRichPresence.Hooks
{
    //Hooks here are not called on pause menu open but rather when game is absolutely paused
    public static class PauseManagerHooks
    {
        public static void AddHooks()
        {
            PauseManager.onPauseStartGlobal += OnGamePaused; // Workaround to pause time on RPC when in pause menu
            PauseManager.onPauseEndGlobal += OnGameUnPaused;
        }

        public static void RemoveHooks()
        {
            PauseManager.onPauseStartGlobal -= OnGamePaused;
            PauseManager.onPauseEndGlobal -= OnGameUnPaused;
        }

        private static void OnGamePaused()
        {
            if (Run.instance != null && CurrentScene != null)
            {
                PresenceUtils.SetStagePresence(CurrentScene, Run.instance, true);
            }
        }

        private static void OnGameUnPaused()
        {
            if (Run.instance != null && CurrentScene != null)
            {
                PresenceUtils.SetStagePresence(CurrentScene, Run.instance);
            }
        }
    }
}