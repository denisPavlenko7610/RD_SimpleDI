using System;
using System.Collections.Generic;
using UnityEngine;

namespace RD_SimpleDI.Runtime.DI
{
    public class DIContainer
    {
        private static readonly Lazy<DIContainer> _lazyInstance = new(() => new DIContainer());
        public static DIContainer Instance => _lazyInstance.Value;

        private readonly Dictionary<Type, Registration> _registrations = new();

        class Registration
        {
            public Func<object> Factory;
            public Lifetime Lifetime;
            public object ObjectInstance;
        }

        public void Bind<TService, TImplementation>(Lifetime lifetime = Lifetime.Singleton)
            where TImplementation : TService, new()
        {
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => new TImplementation(),
                Lifetime = lifetime,
                ObjectInstance = lifetime == Lifetime.Singleton ? new TImplementation() : null
            };
        }

        public void Bind<TService>(TService instance, Lifetime lifetime = Lifetime.Singleton) where TService : class
        {
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => instance,
                Lifetime = lifetime,
                ObjectInstance = instance
            };
        }

        public void Bind<TService>(Func<TService> factory, Lifetime lifetime = Lifetime.Singleton)
            where TService : class
        {
            _registrations[typeof(TService)] = new Registration
            {
                Factory = () => factory(),
                Lifetime = lifetime,
                ObjectInstance = lifetime == Lifetime.Singleton ? factory() : null
            };
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

            return registration.ObjectInstance ??= registration.Factory();
        }

        public void ValidateRegistrations()
        {
            foreach (var serviceType in _registrations.Keys)
            {
                try
                {
                    Resolve(serviceType);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to resolve {serviceType}: {e.Message}");
                }
            }
        }

        public T InstantiateAndBind<T>(T prefab) where T : MonoBehaviour
        {
            T instance = UnityEngine.Object.Instantiate(prefab);
            SetupAfterSpawn(instance);
            return instance;
        }

        public T InstantiateAndBind<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            T instance = UnityEngine.Object.Instantiate(prefab, position, rotation);
            SetupAfterSpawn(instance);
            return instance;
        }

        public T InstantiateAndBind<T>(T prefab, Transform parent, bool needBind) where T : MonoBehaviour
        {
            T instance = UnityEngine.Object.Instantiate(prefab, parent);
            SetupAfterSpawn(instance);
            return instance;
        }

        public T InstantiateAndBind<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent)
            where T : MonoBehaviour
        {
            T instance = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
            SetupAfterSpawn(instance);
            return instance;
        }

        private void SetupAfterSpawn<T>(T instance) where T : MonoBehaviour
        {
            DIInitializer.Instance.InjectDependencies(instance);
            Instance.Bind(instance);
        }
    }
}