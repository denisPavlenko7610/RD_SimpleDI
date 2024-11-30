using System;

namespace RD_SimpleDI.Runtime
{
    public static class GameState
    {
        public static bool IsPaused { get; private set; }
        
        public static Action PauseAction;
        public static Action ResumeAction;
        
        public static void TogglePause()
        {
            if (IsPaused)
            {
                IsPaused = false;
                UnityEngine.Time.timeScale = 1; //Optional. Added to work correctly with TweenUpdater
                ResumeAction?.Invoke();
            }
            else
            {
                IsPaused = true;
                UnityEngine.Time.timeScale = 0; //Optional. Added to work correctly with TweenUpdater
                PauseAction?.Invoke();
            }
        }
    }
}