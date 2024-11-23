using RD_SimpleDI.Runtime.LifeCycle;
using UnityEngine;

namespace DI.Example
{
    public class Person : MonoRunner
    {
        public override void BeforeAwake()
        {
            base.BeforeAwake();
            Debug.Log("before Awake");
        }

        public override void Initialize()
        {
            base.Initialize();
            Show show = new Show();
        }

        public override void Run()
        {
            base.Run();
            Debug.Log("Hello World!");
        }
    }
}