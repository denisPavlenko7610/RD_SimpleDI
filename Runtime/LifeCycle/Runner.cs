using RD_SimpleDI.Runtime.LifeCycle.Interfaces;

namespace RD_SimpleDI.Runtime.LifeCycle
{
    public abstract class Runner : IRunner
    {
        // Automatically register the runner with RunnerUpdater when instantiated.
        protected Runner()
        {
            RunnerUpdater.RegisterRunner(this);
            Init();
            BeforeAwake();
            Initialize();
        }

        ~Runner()
        {
            RunnerUpdater.UnregisterRunner(this);
        }

        void Init()
        {
            GameState.PauseAction += Pause;
            GameState.ResumeAction += Resume;
        }
        
        protected virtual void BeforeAwake(){}
        protected virtual void Initialize(){}
        public virtual void Run(){}
        public virtual void FixedRun(){}
        public virtual void LateRun(){}

        // Pause logic
        private void Pause() => OnPause?.Invoke();
        private void Resume() => OnResume?.Invoke();

        public static event System.Action OnPause;
        public static event System.Action OnResume;
    }
}