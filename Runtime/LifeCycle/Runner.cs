using RD_SimpleDI.Runtime.LifeCycle.Interfaces;

namespace RD_SimpleDI.Runtime.LifeCycle
{
    public abstract class Runner : IRunner
    {
        // Automatically register the runner with RunnerUpdater when instantiated.
        protected Runner()
        {
            RunnerUpdater.RegisterRunner(this);
        }

        ~Runner()
        {
            RunnerUpdater.UnregisterRunner(this);
        }

        public void Init()
        {
            GameState.PauseAction += Pause;
            GameState.ResumeAction += Resume;
            
            Initialize();
        }

        protected virtual void Initialize() {}
        public void Run(float deltaTime) { }
        public void FixedRun(float fixedDeltaTime) { }
        public void LateRun(float lateDeltaTime) { }

        // Pause logic
        private void Pause() => OnPause?.Invoke();
        private void Resume() => OnResume?.Invoke();

        public static event System.Action OnPause;
        public static event System.Action OnResume;
    }
}