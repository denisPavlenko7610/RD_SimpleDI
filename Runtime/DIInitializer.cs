using DI.Attributes;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

// DIInitializer.cs
namespace DI
{
    public class DIInitializer
    {
        static DIInitializer _instance;
        public static DIInitializer Instance => _instance ??= new DIInitializer();

        public void InjectDependencies(MonoBehaviour monoBehaviour)
        {
            Type type = monoBehaviour.GetType();

            // Inject fields
            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
            {
                if (!Attribute.IsDefined(field, typeof(InjectAttribute)))
                    continue;

                object dependency = DIContainer.Instance.Resolve(field.FieldType);
                field.SetValue(monoBehaviour, dependency);
            }

            // Inject methods
            foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
            {
                if (!Attribute.IsDefined(method, typeof(InjectAttribute)))
                    continue;

                object[] parameters = method.GetParameters()
                    .Select(p => DIContainer.Instance.Resolve(p.ParameterType))
                    .ToArray();

                method.Invoke(monoBehaviour, parameters);
            }
        }
    }
}