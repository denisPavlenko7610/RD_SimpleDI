using System;

namespace RD_SimpleDI.Runtime.DI.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field)]
    public class InjectAttribute : Attribute { }
}