using System;
using DI.Interfaces;
using UnityEngine;

namespace RD_Tween.Runtime.LifeCycle
{
    public abstract class MonoRunner : MonoBehaviour
    {
        private async void Start()
        {
            BeforeAwake();
            BeforeStart();
            Initialize();
        }

        private async void OnEnable()
        {
            BeforeEnable();
            Appear();
        }

        private async void Update()
        {
            BeforeUpdate();
            Run();
        }

        private async void FixedUpdate()
        {
            BeforeFixedUpdate();
            FixedRun();
        }

        private async void LateUpdate()
        {
            BeforeLateUpdate();
            LateRun();
        }

        private async void OnDisable()
        {
            BeforeDisable();
            Disappear();
        }

        private async void OnDestroy()
        {
            BeforeDestroy();    
            Delete();
        }

        /// <summary>
        /// Use this methods only once on your entry point
        /// </summary>
        protected virtual void BeforeAwake() {}
        protected virtual void BeforeStart() {}
        protected virtual void BeforeEnable() {}
        protected virtual void BeforeDisable() {}
        protected virtual void BeforeUpdate() {}
        protected virtual void BeforeFixedUpdate() {}
        protected virtual void BeforeLateUpdate() {}
        protected virtual void BeforeDestroy() {}
        
        
        /// <summary>
        /// Methods to use instead standard monobehaviour methods
        /// </summary>

        protected virtual void Initialize() {}
        protected virtual void Appear() {}
        protected virtual void Run() {}
        protected virtual void FixedRun() {}
        protected virtual void LateRun() {}
        protected virtual void Disappear() {}
        protected virtual void Delete() {}
    }
}