using System;

namespace BTP.RoR2Plugin {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal class ModLoadMessageHandlerAttribute : Attribute {
    }
}