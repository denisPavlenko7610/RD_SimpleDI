// using System;
// using System.Threading.Tasks;
// using DI;
// using RD_SimpleDI.Runtime.LifeCycle;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// namespace LikeAGTA.Core
// {
//     public class ProjectContext : MonoRunner
//     {
//         [SerializeField] PlayerInput _playerInput;
//         
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
//         void InitVariables()
//         {
//             _playerInput.actions["Pause"].performed += OnPausePerformed;
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
//             DIContainer.Instance.Bind(_playerInput);
//             // Register global services and dependencies
//             //container.Bind<IAds, AdsService>();
//         }
//
//         public static T Resolve<T>() => DIContainer.Instance.Resolve<T>();
//         
//          private void OnPausePerformed(InputAction.CallbackContext context) => TogglePause();
//
//          protected override void Delete()
//          {
//              base.Delete();
//              _playerInput.actions["Pause"].performed -= OnPausePerformed;
//          }
//     }
// }