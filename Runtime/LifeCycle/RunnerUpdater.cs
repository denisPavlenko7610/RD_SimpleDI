using System.Collections.Generic;
using RD_SimpleDI.Runtime.LifeCycle.Interfaces;
using UnityEngine;

namespace RD_SimpleDI.Runtime.LifeCycle
{
    public class RunnerUpdater : MonoBehaviour
    {
        public static RunnerUpdater Instance { get; private set; }

        private readonly List<IRunner> _runners = new();
        private readonly List<MonoRunner> _monoRunners = new();

        public void Init()
        {
            var managerObject = new GameObject("RunnerUpdater");
            Instance = managerObject.AddComponent<RunnerUpdater>();
            DontDestroyOnLoad(this);
        }

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
                Instance?._monoRunners.Remove(monoRunner);
            }
            else
            {
                Instance?._runners.Remove(runner);
            }
        }

        private void Update()
        {
            if (GameState.IsPaused)
                return;
            
            foreach (IRunner runner in _runners)
            {
                runner?.Run();
            }

            foreach (MonoRunner monoRunner in _monoRunners)
            {
                monoRunner?.Run();
            }
        }

        private void FixedUpdate()
        {
            if (GameState.IsPaused)
                return;
            
            foreach (IRunner runner in _runners)
            {
                runner?.FixedRun();
            }

            foreach (MonoRunner monoRunner in _monoRunners)
            {
                monoRunner?.FixedRun();
            }
        }

        private void LateUpdate()
        {
            if (GameState.IsPaused)
                return;
            
            foreach (IRunner runner in _runners)
            {
                runner.LateRun();
            }

            foreach (MonoRunner monoRunner in _monoRunners)
            {
                monoRunner.LateRun();
            }
        }
    }
}
