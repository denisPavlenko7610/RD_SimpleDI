using DI;
using UnityEngine;

namespace RD_SimpleDI.Runtime.LifeCycle
{
    public abstract class MonoRunner : MonoBehaviour
    {
        private static bool _isPaused;
        
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
            BeforeUpdate();
            Run();
        }
        
        private async void FixedUpdate()
        {
            BeforeFixedUpdate();
            FixedRun();
        }
        
        private async void LateUpdate()
        {
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
           if (!_isPaused)
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
            _isPaused = true;
            OnPause?.Invoke();
        }

        private void Resume()
        {
            _isPaused = false;
            OnResume?.Invoke();
        }
    }
}