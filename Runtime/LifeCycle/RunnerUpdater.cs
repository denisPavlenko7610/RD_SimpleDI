using System.Collections.Generic;
using RD_SimpleDI.Runtime.LifeCycle.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RD_SimpleDI.Runtime.LifeCycle
{
    public class RunnerUpdater : MonoBehaviour
    {
        private static RunnerUpdater _instance;

        private static RunnerUpdater Instance
        {
            get
            {
                if (_instance == null)
                {
                    var managerObject = new GameObject("RunnerUpdater");
                    _instance = managerObject.AddComponent<RunnerUpdater>();
                }
                return _instance;
            }
        }

        private readonly List<IRunner> _runners = new();
        private readonly List<MonoRunner> _monoRunners = new();

        // Register a non-MonoBehaviour Runner
        public static void RegisterRunner(IRunner runner)
        {
            if (runner is MonoRunner monoRunner)
            {
                Instance._monoRunners.Add(monoRunner);
            }
            else
            {
                Instance._runners.Add(runner);
            }
        }

        // Unregister a non-MonoBehaviour Runner
        public static void UnregisterRunner(IRunner runner)
        {
            if (runner is MonoRunner monoRunner)
            {
                Instance?._monoRunners?.Remove(monoRunner);
            }
            else
            {
                Instance?._runners?.Remove(runner);
            }
        }

        private void Update()
        {
            foreach (IRunner runner in _runners)
            {
                if (GameState.IsPaused)
                    continue;
                
                runner.Run();
            }

            foreach (MonoRunner monoRunner in _monoRunners)
            {
                if (GameState.IsPaused)
                    continue;
                
                monoRunner.Run();
            }
        }

        private void FixedUpdate()
        {
            foreach (IRunner runner in _runners)
            {
                if (GameState.IsPaused)
                    continue;
                
                runner.FixedRun();
            }

            foreach (MonoRunner monoRunner in _monoRunners)
            {
                if (GameState.IsPaused)
                    continue;
                
                monoRunner.FixedRun();
            }
        }

        private void LateUpdate()
        {
            foreach (IRunner runner in _runners)
            {
                if (GameState.IsPaused)
                    continue;
                
                runner.LateRun();
            }

            foreach (MonoRunner monoRunner in _monoRunners)
            {
                if (GameState.IsPaused)
                    continue;
                
                monoRunner.LateRun();
            }
        }
        
        // Clean up when the scene is unloaded or when this object is destroyed
        private void OnDestroy()
        {
            // Manually destroy the GameObject and nullify the singleton
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            // Clean up the singleton when the scene is unloaded
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }
    }
}
