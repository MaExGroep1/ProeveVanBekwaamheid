using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Util.Method_Invoker_main
{
    public static class InvokerExtensions
    {
        public static bool MethodHasParameters(this MethodInfo methodInfo) =>
            methodInfo?.GetParameters().Length > 0;
    
        public static IEnumerable<Type> GetUnderlyingParameterTypes(this MethodInfo methodInfo) =>
            methodInfo.GetParameters().Select(pi => pi.ParameterType);
    }
}
