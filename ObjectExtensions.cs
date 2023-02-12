using System.Reflection;

namespace Loxifi
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Invokes a method on the object using the given name and parameters
        /// </summary>
        /// <param name="o">The object to invoke the method on</param>
        /// <param name="MethodName">The name of the method</param>
        /// <param name="flags">Binding flags used to find the method</param>
        /// <param name="Parameters">Any method parameters</param>
        public static void Invoke(this object o, BindingFlags flags, string MethodName, params object[] Parameters) => _ = o.Invoke<object>(flags, MethodName, Parameters);

        /// <summary>
        /// Invokes a method on the object using the given name and parameters
        /// </summary>
        /// <param name="o">The object to invoke the method on</param>
        /// <param name="MethodName">The name of the method</param>
        /// <param name="Parameters">Any method parameters</param>
        public static T Invoke<T>(this object o, string MethodName, params object[] Parameters) => o.Invoke<T>(BindingFlags.Public | BindingFlags.Instance, MethodName, Parameters);

        /// <summary>
        /// Invokes a method on the object using the given name and parameters
        /// </summary>
        /// <typeparam name="T">The method return type</typeparam>
        /// <param name="o">The object to invoke the method on</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="flags">Binding flags used to find the method</param>
        /// <param name="suppliedParameters">Any method parameters</param>
        /// <returns>The method return</returns>
        public static T Invoke<T>(this object o, BindingFlags flags, string methodName, params object[] suppliedParameters)
        {
            if (o is null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            if (suppliedParameters is null)
            {
                throw new ArgumentNullException(nameof(suppliedParameters));
            }

            Type t = o.GetType();

            List<MethodInfo> methods = t.GetMethods(flags).ToList();

            if (!methods.Any())
            {
                throw new NotImplementedException($"No methods found on object with type {t}");
            }

            methods = methods.Where(m => m.Name == methodName).ToList();

            if (!methods.Any())
            {
                throw new NotImplementedException($"Method with name {methodName} not found on object with type {t}");
            }

            List<MethodInfo> matching = new();

            foreach (MethodInfo m in methods)
            {
                List<ParameterInfo> parameters = m.GetParameters().ToList();

                bool isMatch = true;

                for (int parameterIndex = 0; parameterIndex < parameters.Count && isMatch; parameterIndex++)
                {
                    ParameterInfo expectedParameter = parameters[parameterIndex];

                    if (suppliedParameters.Length <= parameterIndex)
                    {
                        if (!expectedParameter.IsOptional)
                        {
                            isMatch = false;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    object suppliedParameter = suppliedParameters[parameterIndex];
                    Type expectedParameterType = expectedParameter.ParameterType;

                    if (suppliedParameter is null)
                    {
                        if (expectedParameterType.IsPrimitive || !expectedParameterType.IsClass)
                        {
                            isMatch = false;
                            break;
                        }
                    }
                    else
                    {
                        Type suppliedParameterType = suppliedParameter.GetType();

                        if (!expectedParameterType.IsAssignableFrom(suppliedParameterType))
                        {
                            isMatch = false;
                            break;
                        }
                    }
                }

                matching.Add(m);
            }

            if (matching.Count == 0)
            {
                throw new NotImplementedException($"Could not find method matching signature {methodName}({string.Join(", ", suppliedParameters.Select(p => p is null ? "object?" : p.GetType().GetDeclaration()))})");
            }

            if (matching.Count > 1)
            {
                throw new AmbiguousMatchException($"Multiple methods matching signature {methodName}({string.Join(", ", suppliedParameters.Select(p => p is null ? "object?" : p.GetType().GetDeclaration()))})");
            }

            List<object> parametersToPass = suppliedParameters.ToList();

            int expectedParameterCount = matching[0].GetParameters().Length;

            while (parametersToPass.Count < expectedParameterCount)
            {
                parametersToPass.Add(Type.Missing);
            }

            object result = matching[0].Invoke(o, parametersToPass.ToArray());

            return result is not T
                ? throw new InvalidCastException($"Method {methodName} returned type {(result is null ? "null" : result.GetType().ToString())} but a return type of {typeof(T)} was expected.")
                : (T)result;
        }
    }
}
