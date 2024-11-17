using DI;
using UnityEngine;

namespace RD_SimpleDI.Runtime.LifeCycle
{
    public abstract class MonoRunner : MonoBehaviour
    {
        protected static bool IsPaused { get; private set; }
        
        public static event System.Action OnPause;
        public static event System.Action OnResume;
        
        private async void Awake()
        {
            BeforeAwake();
            DIInitializer.Instance.InjectDependencies(this);
        }
        
        private async void Start()
        {
            BeforeStart();
            Initialize();
        }

        private async void OnEnable()
        {
            BeforeEnable();
            Appear();
        }

        private async void Update()
        {
            if (IsPaused)
                return;
            
            BeforeUpdate();
            Run();
        }
        
        private async void FixedUpdate()
        {
            if (IsPaused)
                return;
            
            BeforeFixedUpdate();
            FixedRun();
        }
        
        private async void LateUpdate()
        {
            if (IsPaused)
                return;
            
            BeforeLateUpdate();
            LateRun();
        }
        
        private async void OnDisable()
        {
            BeforeDisable();
            Disappear();
        }
        
        private async void OnDestroy()
        {
            BeforeDestroy();    
            Delete();
        }

        /// <summary>
        /// Use this methods only once on your entry point
        /// </summary>
        protected virtual void BeforeAwake() {}
        protected virtual void BeforeStart() {} 
        protected virtual void BeforeEnable() {}
        protected virtual void BeforeDisable() {}
        protected virtual void BeforeUpdate() {}
        protected virtual void BeforeFixedUpdate() {}
        protected virtual void BeforeLateUpdate() {}
        protected virtual void BeforeDestroy() {}
        
        
        /// <summary>
        /// Methods to use instead standard monobehaviour methods
        /// </summary>

        protected virtual void Initialize() {}
        protected virtual void Appear() {}
        protected virtual void Run() {}
        protected virtual void FixedRun() {}
        protected virtual void LateRun() {}
        protected virtual void Disappear() {}
        protected virtual void Delete() {}
        
        
        
        /// <summary>
        /// Custom lifecycle methods. Use these methods for pause and resume in game
        /// </summary>
        
       protected void TogglePause()
       {
           if (!IsPaused)
           {
               Pause();
           }
           else
           {
               Resume();
           }
       }
        
        private void Pause()
        {
            IsPaused = true;
            OnPause?.Invoke();
        }

        private void Resume()
        {
            IsPaused = false;
            OnResume?.Invoke();
        }
    }
}