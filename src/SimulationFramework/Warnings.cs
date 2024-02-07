using System.Reflection;

namespace SimulationFramework;
internal static class Warnings
{
    public static string TypeDoesNotImplementInterface(Type type, Type interfaceType, string context = "") => $"Type \"{type}\" does not implement \"{interfaceType}\". {context}";
    public static string TypeDoesNotHaveParameterlessConstructor(Type type, string context = "") => $"Type \"{type}\" does not provide a parameterless constructor. {context}";
    public static string TypeDoesNotHavePublicStaticMethod(Type type, string methodName, string context = "") => $"Type \"{type}\" does not have a public static method named {methodName}. {context}";
    public static string MethodExpectedReturnType(MethodInfo method, Type returnType, string context = "") => $"Method \"{Format(method)}\" does not have return type \"{returnType}\". {context}";
    public static string MethodExpectedNonGeneric(MethodInfo method, string context = "") => $"Method \"{Format(method)}\" is generic. {context}";
    public static string MethodInvocationFailed(MethodInfo method, string context = "") => $"Could not invoke method \"{Format(method)}\". {context}";
    public static string ConstructorInvocationFailed(MethodInfo method, string context = "") => $"Could not invoke method \"{Format(method)}\". {context}";
    public static string DuplicateMessageSubscription(Delegate subscriber, Type messageType, string context = "") => $"Delegate \"{subscriber}\" is already subscribed to the message type \"{messageType}\". {context}";

    private static string Format(MethodInfo method) => $"{method.DeclaringType?.Name ?? "[global]"}{method}";
}