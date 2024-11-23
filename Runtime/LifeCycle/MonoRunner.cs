using DI;
using RD_SimpleDI.Runtime.LifeCycle.Interfaces;
using UnityEngine;

namespace RD_SimpleDI.Runtime.LifeCycle
{
    public abstract class MonoRunner : MonoBehaviour, IRunner
    {
        // Automatically register the MonoRunner when Awake is called.
        protected virtual void Awake()
        {
            GameState.PauseAction += Pause;
            GameState.ResumeAction += Resume;
            
            RunnerUpdater.RegisterRunner(this);
            DIInitializer.Instance.InjectDependencies(this); // Inject dependencies for MonoBehaviour
            BeforeAwake();
            Initialize();
        }

        protected virtual void Start()
        {
            BeforeStart();
            Appear();
        }

        protected virtual void OnEnable()
        {
            BeforeEnable();
        }

        protected virtual void OnDisable()
        {
            BeforeDisable();
            Disappear();
        }

        protected virtual void OnDestroy()
        {
            BeforeDestroy();
            Delete();
            RunnerUpdater.UnregisterRunner(this);
        }

        /// <summary>
        /// Hooks for MonoBehaviour-specific lifecycle events
        /// </summary>
        protected virtual void BeforeAwake() {}
        protected virtual void BeforeStart() {}
        protected virtual void BeforeEnable() {}
        protected virtual void BeforeDisable() {}
        // protected virtual void BeforeUpdate() {}
        // protected virtual void BeforeFixedUpdate() {}
        // protected virtual void BeforeLateUpdate() {}
        protected virtual void BeforeDestroy() {}

        // These methods will be overridden to implement specific behavior
        protected virtual void Initialize(){}
        protected virtual void Appear(){}
        public virtual void Run(){}
        public virtual void FixedRun(){}
        public virtual void LateRun(){}
        protected virtual void Disappear(){}
        protected virtual void Delete(){}
        
        // Pause logic
        private void Pause() => OnPause?.Invoke();
        private void Resume() => OnResume?.Invoke();

        public static event System.Action OnPause;
        public static event System.Action OnResume;
    }
}
