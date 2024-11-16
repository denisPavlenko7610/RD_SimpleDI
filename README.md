# RD_SimpleDI
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
using DI.Interfaces;
using Example;
using UnityEngine;

//Exemple SceneContext class
public class SceneContext : MonoBehaviour
{
    [SerializeField] Box _box;
    [SerializeField] Cube _cube;

    private void Awake()
    {
        InitializeBindings();
        injectDependencies();
    }
    
    void InitializeBindings()
    {
        //DIContainer.Instance.Bind<IAudioService, AudioService>(); //Bind non mono
        DIContainer.Instance.Bind<IAudioService, VideoService>(); //Bind non mono
        
        // var audioService = new AudioService();
        // DIContainer.Instance.Bind(audioService); //bind non mono
        
        //DIContainer.Instance.Bind(_cube); //bind mono //Lifetime.Singleton by default
        
        DIContainer.Instance.Bind<ICube>(_box); //bind mono
       
        //DIContainer.Instance.Bind<ICube>(_box, Lifetime.Transient); //bind mono
        //DIContainer.Instance.Bind<ICube>(_cube, Lifetime.Cached); //bind mono
        //DIContainer.Instance.Bind<ICube>(_cube, Lifetime.Singleton); //bind mono
    }
    
    void injectDependencies()
    {
        foreach (MonoBehaviour monoBehaviour in FindObjectsOfType<MonoBehaviour>(true))
        {
            DIInitializer.Instance.InjectDependencies(monoBehaviour);
            if (monoBehaviour is IInitializable initializable)
            {
                initializable.Initialize();
            }
        }
    }
}
```

# Create SceneContext class on level scene
- Use DIContainer.Instance.Bind to bins non monobehaviour or monobehaviour classes with or without interfaces
- Use [Inject] attribute to resolve dependency
- You can use IInitializable interface instead Awake or Start monobehaviour methods
- If you want to resolve dependencies in runtime use this approach
  
```C#
DIContainer.Instance.InstantiateAndInject(_cube2);
DIContainer.Instance.InstantiateAndInject(_cube2, needInitialize: true);
```

- Also you can manually resolve

```C#
Cube3 cube3 = new Cube3(DIContainer.Instance.Resolve<AdsService>());
```

```C#
public class AudioPlayer : MonoBehaviour, IInitializable
{
    [SerializeField] Cube2 _cube2;
    
    IAudioService _audioService;
    ICube _cube;

    [Inject]
    void initialize(IAudioService audioService, ICube cube)
    {
        _audioService = audioService;
        _cube = cube;
    }
    
    public void Initialize()
    {
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
  public class Player : MonoBehaviour
    {
        IAds _ads;

        public void Awake()
        {
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

public class ProjectContext : MonoBehaviour
{
    [SerializeField] AdsService _adsService; 
    
    void Awake()
    {
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

Also use methods BeforeAwake, BeforeStart, BeforeUpdate and similar only once in your entry point to call some logic before these methods

