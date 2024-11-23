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
                ResumeAction?.Invoke();
            }
            else
            {
                IsPaused = true;
                PauseAction?.Invoke();
            }
        }
    }
}