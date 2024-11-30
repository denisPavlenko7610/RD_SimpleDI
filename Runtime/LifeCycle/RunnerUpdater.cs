using System.Collections.Generic;
using _Packages.RD_SimpleDI.Runtime.LifeCycle.Interfaces;
using RD_SimpleDI.Runtime;
using RD_SimpleDI.Runtime.LifeCycle.Interfaces;
using UnityEngine;

namespace _Packages.RD_SimpleDI.Runtime.LifeCycle
{
    public class RunnerUpdater : MonoBehaviour
    {
        private static RunnerUpdater _instance;
    
        private static bool _isShuttingDown;
        private bool _isPaused;
        
        private readonly HashSet<IRunner> _runners = new();
        
        // Pause logic
        private void Pause()
        {
            OnPause?.Invoke();
            _isPaused = true;
        }

        private void Resume()
        {
            OnResume?.Invoke();
            _isPaused = false;
        }

        public static event System.Action OnPause;
        public static event System.Action OnResume;

        private static RunnerUpdater Instance
        {
            get
            {
                if (_isShuttingDown) 
                    return null;
            
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<RunnerUpdater>();
                }
                return _instance;
            }
        }
        
        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (_isPaused)
                return;

            foreach (var runner in _runners) 
                runner.Run(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_isPaused)
                return;

            foreach (var runner in _runners)
                runner.FixedRun(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (_isPaused)
                return;

            foreach (var runner in _runners)
                runner.LateRun(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (_instance != this) 
                return;
            
            _runners.Clear();
            _isShuttingDown = true;
            _instance = null;
        }

        public static void RegisterRunner(IRunner runner)
        {
            RegisterPause(runner);
            Instance._runners.Add(runner);
        }
        
        public static void UnregisterRunner(IRunner runner)
        {
            UnregisterPause(runner);
            Instance?._runners.Remove(runner);
        }

        private static void RegisterPause(IRunner runner)
        {
            if (runner is IPause iPause)
                GameState.PauseAction += iPause.Pause;

            if (runner is IResume iResume)
                GameState.ResumeAction += iResume.Resume;
        }
        
        private static void UnregisterPause(IRunner runner)
        {
            if (runner is IPause iPause)
                GameState.PauseAction -= iPause.Pause;

            if (runner is IResume iResume)
                GameState.ResumeAction -= iResume.Resume;
        }
    }
}
