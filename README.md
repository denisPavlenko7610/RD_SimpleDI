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

public class Bootstrap : MonoBehaviour
{
    [SerializeField] Box _box;
    [SerializeField] Cube _cube;

    private void Awake()
    {
        init();
        injectDependencies();
    }
    
    void init()
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
    
    private void injectDependencies()
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

# Create bootstrap class
- Use DIContainer.Instance.Bind to bins non monobehaviour or monobehaviour classes with or without interfaces
- Use [Inject] attribute to resolve dependency
- You can use IInitializable interface instead Awake or Start monobehaviour methods
- If you want to resolve dependencies in runtime use this approach
```C#
DIContainer.Instance.InstantiateAndInject(_cube2);
```

```C#
public class AudioPlayer : MonoBehaviour, IInitializable
{
    [SerializeField] Cube2 _cube2;
    
    private IAudioService _audioService;
    private ICube _cube;

    [Inject]
    private void initialize(IAudioService audioService, ICube cube)
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

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1);
        Cube2 obj = DIContainer.Instance.InstantiateAndInject(_cube2);
    }
}
