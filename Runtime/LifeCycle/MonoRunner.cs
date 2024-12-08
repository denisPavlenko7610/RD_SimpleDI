using RD_SimpleDI.Runtime.DI;
using RD_SimpleDI.Runtime.LifeCycle.Interfaces;
using UnityEngine;

namespace RD_SimpleDI.Runtime.LifeCycle
{
    public abstract class MonoRunner : MonoBehaviour, IRunner
    {
        void Awake()
        {
            RunnerUpdater.RegisterRunner(this);
            DIInitializer.Instance.InjectDependencies(this); // Inject dependencies for MonoBehaviour
            Initialize();
        }
        
        //
        // protected virtual void OnEnable()
        // {
        //     Appear();
        // }
        //
        // protected virtual void OnDisable()
        // {
        //     Disappear();
        // }
        //
        protected virtual void OnDestroy()
        {
            Delete();
            RunnerUpdater.UnregisterRunner(this);
        }
        
        //These methods will be overridden to implement specific behavior
        protected virtual void Initialize(){}
        //protected virtual void Appear(){}
        public virtual void Run(float deltaTime){}
        public virtual void FixedRun(float fixedDeltaTime){}
        public virtual void LateRun(float lateDeltaTime){}
        //protected virtual void Disappear(){}
        protected virtual void Delete(){}
    }
}
