using System;
using System.Collections.Generic;
using RD_Tween.Runtime.LifeCycle;
using UnityEngine;

namespace DI
{
    public class DIContainer
    {
        static DIContainer _instance;
        public static DIContainer Instance => _instance ??= new DIContainer();

        private readonly Dictionary<Type, Registration> _registrations = new();

        class Registration
        {
            public Func<object> Factory;
            public Lifetime Lifetime;
            public object ObjectInstance;
        }

        // Bind by interface or base class
        public void Bind<TService, TImplementation>(Lifetime lifetime = Lifetime.Transient) where TImplementation : TService, new()
        {
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => new TImplementation(),
                Lifetime = lifetime,
            };
        }

        public void Bind<TService>(TService instance, Lifetime lifetime = Lifetime.Singleton) where TService : class
        {
            if (instance is MonoBehaviour monoBehaviour && lifetime == Lifetime.Transient)
            {
                _registrations[typeof(TService)] = new Registration
                {
                    Factory = () => UnityEngine.Object.Instantiate(monoBehaviour),
                    Lifetime = lifetime
                };
            }
            else
            {
                _registrations[typeof(TService)] = new Registration
                {
                    Factory = () => instance,
                    Lifetime = lifetime,
                    ObjectInstance = lifetime == Lifetime.Singleton || lifetime == Lifetime.Cached ? instance : null
                };
            }
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type serviceType)
        {
            if (!_registrations.TryGetValue(serviceType, out var registration))
            {
                throw new InvalidOperationException($"Service of type {serviceType} is not registered.");
            }

            return registration.Lifetime switch
            {
                Lifetime.Singleton => registration.ObjectInstance ??= registration.Factory(),
                Lifetime.Cached => registration.ObjectInstance ??= registration.Factory(),
                Lifetime.Transient => registration.Factory(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // Instantiate and inject dependencies
        public T InstantiateAndBind<T>(T prefab) where T : MonoRunner
        {
            T instance = UnityEngine.Object.Instantiate(prefab);
            SetupAfterSpawn(instance);
            return instance;
        }

        public T InstantiateAndBind<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoRunner
        {
            T instance = UnityEngine.Object.Instantiate(prefab, position, rotation);
            SetupAfterSpawn(instance);
            return instance;
        }

        public T InstantiateAndBind<T>(T prefab, Transform parent, bool needBind) where T : MonoRunner
        {
            T instance = UnityEngine.Object.Instantiate(prefab, parent);
            SetupAfterSpawn(instance);
            return instance;
        }

        public T InstantiateAndBind<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : MonoRunner
        {
            T instance = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
            SetupAfterSpawn(instance);
            return instance;
        }

        void SetupAfterSpawn<T>(T instance) where T : MonoRunner
        {
            DIInitializer.Instance.InjectDependencies(instance);
            Instance.Bind(instance);
        }
    }
}