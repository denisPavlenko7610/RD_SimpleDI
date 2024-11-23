// using System;
// using System.Threading.Tasks;
// using DI;
// using RD_SimpleDI.Runtime;
// using RD_SimpleDI.Runtime.LifeCycle;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace LikeAGTA.Core
// {
//     public class ProjectContext : MonoRunner
//     {
//         private InputAction _pauseAction;
//
//         protected override async void BeforeAwake()
//         {
//             try
//             {
//                 base.BeforeAwake();
//                 InitializeBindings();
//                 SetUnityLogStatus();
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
//             _pauseAction = InputSystem.actions.FindAction("Pause");
//             _pauseAction.performed += OnPausePerformed;
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
//         void SetUnityLogStatus()
//         {
// #if UNITY_EDITOR
//             Debug.unityLogger.logEnabled = true;
// #else
//             Debug.unityLogger.logEnabled = false;
// #endif
//         }
//
//
//         public static T Resolve<T>() => DIContainer.Instance.Resolve<T>();
//
//         private void OnPausePerformed(InputAction.CallbackContext context) => GameState.TogglePause();
//
//         protected override void Delete()
//         {
//             base.Delete();
//             _pauseAction.performed -= OnPausePerformed;
//         }
//     }
// }