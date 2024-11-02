using DI.Attributes;
using DI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DI
{
    public class DIContainer
    {
        static DIContainer _instance;
        public static DIContainer Instance => _instance ??= new DIContainer();

        private readonly Dictionary<Type, Registration> _registrations = new Dictionary<Type, Registration>();

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
                Lifetime.Transient => registration.Factory(), // New instance every time for Transient
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // Instantiate and inject dependencies
        public T InstantiateAndInject<T>(T prefab) where T : MonoBehaviour, IInitializable
        {
            T instance = UnityEngine.Object.Instantiate(prefab);
            InjectDependencies(instance);
            instance.Initialize();
            return instance;
        }

        // Overload with position and rotation
        public T InstantiateAndInject<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour, IInitializable
        {
            T instance = UnityEngine.Object.Instantiate(prefab, position, rotation);
            InjectDependencies(instance);
            instance.Initialize();
            return instance;
        }

        // Overload with parent
        public T InstantiateAndInject<T>(T prefab, Transform parent) where T : MonoBehaviour, IInitializable
        {
            T instance = UnityEngine.Object.Instantiate(prefab, parent);
            InjectDependencies(instance);
            instance.Initialize();
            return instance;
        }

        // Overload with position, rotation, and parent
        public T InstantiateAndInject<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : MonoBehaviour, IInitializable
        {
            T instance = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
            InjectDependencies(instance);
            instance.Initialize();
            return instance;
        }

        // Inject dependencies into any object
        void InjectDependencies(object instance)
        {
            var type = instance.GetType();

            // Inject fields marked with [Inject]
            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
            {
                if (Attribute.IsDefined(field, typeof(InjectAttribute)))
                {
                    var dependency = Resolve(field.FieldType);
                    field.SetValue(instance, dependency);
                }
            }

            // Inject methods marked with [Inject]
            foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
            {
                if (Attribute.IsDefined(method, typeof(InjectAttribute)))
                {
                    var parameters = method.GetParameters()
                        .Select(p => Resolve(p.ParameterType))
                        .ToArray();

                    method.Invoke(instance, parameters);
                }
            }
        }
    }
}
