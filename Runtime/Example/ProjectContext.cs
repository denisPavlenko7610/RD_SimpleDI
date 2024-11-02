// using DI;
// using DI.Interfaces;
// using Example;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// //Exemple ProjectContext class
// public class ProjectContext : MonoBehaviour
// {
//     [SerializeField] AdsService _adsService; 
//     
//     void Awake()
//     {
//         InitializeBindings();
//         DontDestroyOnLoad(gameObject);
//
//         SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).buildIndex + 1);
//     }
//
//     void InitializeBindings()
//     {
//         var container = DIContainer.Instance;
//
//         // Register global services and dependencies
//         //container.Bind<IAds, AdsService>();
//         container.Bind<IAds>(_adsService);
//         //container.Bind(_adsService);
//     }
//
//     public static T Resolve<T>() => DIContainer.Instance.Resolve<T>();
// }