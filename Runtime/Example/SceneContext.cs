// using DI;
// using DI.Interfaces;
// using Example;
// using UnityEngine;
//
// //Exemple SceneContext class
// public class SceneContext : MonoBehaviour
// {
//     [SerializeField] Box _box;
//     [SerializeField] Cube _cube;
//
//     private void Awake()
//     {
//         InitializeBindings();
//         injectDependencies();
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
//     void injectDependencies()
//     {
//         foreach (MonoBehaviour monoBehaviour in FindObjectsOfType<MonoBehaviour>(true))
//         {
//             DIInitializer.Instance.InjectDependencies(monoBehaviour);
//             if (monoBehaviour is IInitializable initializable)
//             {
//                 initializable.Initialize();
//             }
//         }
//     }
// }