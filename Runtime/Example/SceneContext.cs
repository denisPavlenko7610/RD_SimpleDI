// using DI;
// using RD_SimpleDI.Runtime.LifeCycle;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// //Exemple SceneContext class
// public class SceneContext : MonoRunner
// {
//     [SerializeField] Box _box;
//     [SerializeField] Cube _cube;
//     
//     private PlayerInput _playerInput;
//
//     protected override void BeforeAwake()
//     {
//         base.Initialize();
//         InitializeBindings();
//         InitVariables();
//     }
//
//     void InitVariables()
//     {
//         _playerInput = GetComponent<PlayerInput>();
//         _playerInput.actions["Pause"].performed += OnPausePerformed;
//     }
//     
//     void InitializeBindings()
//     {
//         //DIContainer.Instance.Bind<IAudioService, AudioService>(); //Bind non mono
//         DIContainer.Instance.Bind<IAudioService, VideoService>(); //Bind non mono
//         
//         // var audioService = new AudioService();
//         // DIContainer.Instance.Bind(audioService); //bind non mono
//         
//         //DIContainer.Instance.Bind(_cube); //bind mono
//         
//         DIContainer.Instance.Bind<ICube>(_box); //bind mono
//        
//         //DIContainer.Instance.Bind<ICube>(_box, Lifetime.Transient); //bind mono
//         //DIContainer.Instance.Bind<ICube>(_cube, Lifetime.Cached); //bind mono
//         //DIContainer.Instance.Bind<ICube>(_cube, Lifetime.Singleton); //bind mono
//     }
//     
//     private void OnPausePerformed(InputAction.CallbackContext context)
//     {
//         TogglePause();
//     }
//
//     void TogglePause()
//     {
//         if (IsPaused)
//         {
//             Pause();
//         }
//         else
//         {
//             Resume();
//         }
//     }
// }