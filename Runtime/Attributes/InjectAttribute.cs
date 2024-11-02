using System;

namespace DI.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field)]
    public class InjectAttribute : Attribute { }
}