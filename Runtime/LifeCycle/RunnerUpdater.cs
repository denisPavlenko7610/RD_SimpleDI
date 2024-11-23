using System.Collections.Generic;
using RD_SimpleDI.Runtime;
using RD_SimpleDI.Runtime.LifeCycle;
using RD_SimpleDI.Runtime.LifeCycle.Interfaces;
using UnityEngine;

public class RunnerUpdater : MonoBehaviour
{
    private static RunnerUpdater _instance;
    
    private static bool _isShuttingDown = false;
    public static RunnerUpdater Instance
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
    private readonly List<MonoRunner> _monoRunners = new();

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
        if (runner is MonoRunner monoRunner)
        {
            Instance._monoRunners.Add(monoRunner);
        }
        else
        {
            Instance._runners.Add(runner);
        }
    }

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
        if (GameState.IsPaused) return;

        foreach (var runner in _runners) runner?.Run();
        foreach (var monoRunner in _monoRunners) monoRunner?.Run();
    }

    private void FixedUpdate()
    {
        if (GameState.IsPaused) return;

        foreach (var runner in _runners) runner?.FixedRun();
        foreach (var monoRunner in _monoRunners) monoRunner?.FixedRun();
    }

    private void LateUpdate()
    {
        if (GameState.IsPaused) return;

        foreach (var runner in _runners) runner?.LateRun();
        foreach (var monoRunner in _monoRunners) monoRunner?.LateRun();
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _runners.Clear();
            _monoRunners.Clear();
            _instance = null;
            _isShuttingDown = true;
        }
    }
}
