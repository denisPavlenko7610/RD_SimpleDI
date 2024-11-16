// using System;
// using System.Threading.Tasks;
// using DI;
// using RD_Tween.Runtime.LifeCycle;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// namespace LikeAGTA.Core
// {
//     public class ProjectContext : MonoRunner
//     {
//         protected override async void BeforeAwake()
//         {
//             try
//             {
//                 base.BeforeAwake();
//                 InitializeBindings();
//                 DontDestroyOnLoad(gameObject);
//
//                 await LoadMainScene();
//             }
//             catch (Exception e)
//             {
//                 throw;
//             }
//         }
//         
//         private async Task LoadMainScene()
//         {
//             AsyncOperation loadEnvironmentTask = SceneManager.LoadSceneAsync("Environment", LoadSceneMode.Additive);
//             AsyncOperation loadPlayerTask = SceneManager.LoadSceneAsync("Player", LoadSceneMode.Additive);
//             
//             while (!loadEnvironmentTask.isDone || !loadPlayerTask.isDone)
//             {
//                 await Task.Yield();
//             }
//
//             await SceneManager.UnloadSceneAsync("Bootstrap");
//         }
//
//         void InitializeBindings()
//         {
//             var container = DIContainer.Instance;
//
//             // Register global services and dependencies
//             //container.Bind<IAds, AdsService>();
//         }
//
//         public static T Resolve<T>() => DIContainer.Instance.Resolve<T>();
//     }
// }