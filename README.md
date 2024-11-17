_# RD_SimpleDI
Smallest and simple di for unity

Unity Dependency Injection (DI) Framework
A simple Dependency Injection framework for Unity, supporting various lifetimes (Singleton, Transient, Cached) and enabling clean and efficient dependency management for both MonoBehaviour and non-MonoBehaviour types.

##Features
Easy Dependency Registration and Resolution
Multiple Lifetime Scopes: Singleton, Transient, Cached
Automatic Field and Method Injection via [Inject] attribute

# Setup

```C#
using DI;
using RD_SimpleDI.Runtime.LifeCycle;
using UnityEngine;
using UnityEngine.UIElements;

//Exemple SceneContext class
public class SceneContext : MonoRunner
{
    [SerializeField] Box _box;
    [SerializeField] Cube _cube;
    
    private PlayerInput _playerInput;

    protected override void BeforeAwake()
    {
        base.Initialize();
        InitializeBindings();
        InitVariables();
    }

    void InitVariables()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Pause"].performed += OnPausePerformed;
    }
    
    void InitializeBindings()
    {
        //DIContainer.Instance.Bind<IAudioService, AudioService>(); //Bind non mono
        DIContainer.Instance.Bind<IAudioService, VideoService>(); //Bind non mono
        
        // var audioService = new AudioService();
        // DIContainer.Instance.Bind(audioService); //bind non mono
        
        //DIContainer.Instance.Bind(_cube); //bind mono
        
        DIContainer.Instance.Bind<ICube>(_box); //bind mono
       
        //DIContainer.Instance.Bind<ICube>(_box, Lifetime.Transient); //bind mono
        //DIContainer.Instance.Bind<ICube>(_cube, Lifetime.Cached); //bind mono
        //DIContainer.Instance.Bind<ICube>(_cube, Lifetime.Singleton); //bind mono
    }
    
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    void TogglePause()
    {
        if (IsPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
}
```

# Create SceneContext class on level scene
- Use DIContainer.Instance.Bind to bins non monobehaviour or monobehaviour classes with or without interfaces
- Use [Inject] attribute to resolve dependency
- You can use MonoRunner instead of standard monobehaviour methods. See below
- If you want to resolve dependencies in runtime use this approach
- Override Pause and Resume if you want to do some specific on it. Also choose the key and subscribe on it to
toggle pause.
  
```C#
DIContainer.Instance.InstantiateAndInject(_cube2);
DIContainer.Instance.InstantiateAndInject(_cube2, needInitialize: true);
```

- Also you can manually resolve

```C#
Cube3 cube3 = new Cube3(DIContainer.Instance.Resolve<AdsService>());
```

```C#
public class AudioPlayer : MonoRunner
{
    [SerializeField] Cube2 _cube2;
    
    IAudioService _audioService;
    ICube _cube;

    [Inject]
    void Construct(IAudioService audioService, ICube cube)
    {
        _audioService = audioService;
        _cube = cube;
    }
    
    protected override void Initialize()
    {
        base.Initialize();
        _audioService.PlaySound();
        _cube.Move();
        StartCoroutine(nameof(Spawn));
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1);
        Cube2 obj = DIContainer.Instance.InstantiateAndInject(_cube2);
    }
}
```

# Use Project context logic

- To resolve dependencies in this way use ProjectContext.Resolve<IAds>();
  
```C#
  public class Player : MonoRunner
    {
        IAds _ads;

        protected override void Initialize()
        {
            base.Initialize();
            _ads = ProjectContext.Resolve<IAds>();
            _ads.Show();
        }
    }
```

```C#
using DI;
using DI.Interfaces;
using Example;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectContext : MonoRunner
{
    [SerializeField] AdsService _adsService; 
    
    protected override void BeforeAwake()
    {
        base.BeforeAwake();
        InitializeBindings();
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).buildIndex + 1);
    }

    void InitializeBindings()
    {
        var container = DIContainer.Instance;

        // Register global services and dependencies
        //container.Bind<IAds, AdsService>();
        container.Bind<IAds>(_adsService);
        //container.Bind(_adsService);
    }

    public static T Resolve<T>() => DIContainer.Instance.Resolve<T>();
}
```

# Add MonoRunner instead MonoBehaviour
use instead Awake and Start - Initialize,
instead Update, FixedUpdate, LateUpdate - Run, FixedRun, LateRun
instead OnEnable - Appear
instead OnDisable - Disappear
instead OnDestroy - Delete

Also use methods BeforeAwake, BeforeStart, BeforeUpdate and similar only once in your entry point to call some logic before these methods_

