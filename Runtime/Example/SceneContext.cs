// using DI;
// using RD_Tween.Runtime.LifeCycle;
// using UnityEngine;
//
// //Exemple SceneContext class
// public class SceneContext : MonoRunner
// {
//     [SerializeField] Box _box;
//     [SerializeField] Cube _cube;
//
//     protected override void BeforeAwake()
//     {
//         base.Initialize();
//         InitializeBindings();
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
// }