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
        
                
        // Pause logic
        private static void Pause() => OnPause?.Invoke();
        private static void Resume() => OnResume?.Invoke();

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
                    var existingObject = FindAnyObjectByType<RunnerUpdater>();
                    if (existingObject != null)
                    {
                        _instance = existingObject;
                    }
                    else
                    {
                        var managerObject = new GameObject("RunnerUpdater");
                        _instance = managerObject.AddComponent<RunnerUpdater>();
                        DontDestroyOnLoad(managerObject);
                    }
                }
                return _instance;
            }
        }


        private readonly List<IRunner> _runners = new();

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

        public static void RegisterRunner(IRunner runner)
        {
            RegisterPause(runner);
                Instance._runners.Add(runner);
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

        public static void UnregisterRunner(IRunner runner)
        {
            UnregisterPause(runner);
            Instance?._runners.Remove(runner);
        }

        private void Start()
        {
            foreach (IRunner runner in _runners)
            {
                runner.Init();
            }
        }

        private void Update()
        {
            if (GameState.IsPaused)
                return;

            foreach (var runner in _runners) 
                runner?.Run(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (GameState.IsPaused)
                return;

            foreach (var runner in _runners)
                runner?.FixedRun(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (GameState.IsPaused)
                return;

            foreach (var runner in _runners)
                runner?.LateRun(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _runners.Clear();
                _instance = null;
                _isShuttingDown = true;
            }
        }
    }
}
