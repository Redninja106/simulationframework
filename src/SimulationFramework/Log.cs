using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
internal static class Log
{
    [Conditional("DEBUG")]
    public static void Error(string message, bool shouldThrow = true)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine($"ERROR: {message}");

        Console.ForegroundColor = prevColor;

        if (shouldThrow)
            throw new Exception(message);
    }

    [Conditional("DEBUG")]
    public static void Assert(bool condition, [CallerArgumentExpression("condition")] string? message = null)
    {
        if (condition)
            return;

        var result = FormatAssertion(new StackFrame(1, true), message);
        Error(result);
    }

    [Conditional("DEBUG")]
    public static void Warning(string message)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine($"WARNING: {message}");

        Console.ForegroundColor = prevColor;
    }

    [Conditional("DEBUG")]
    public static void AssertWarning(bool condition, [CallerArgumentExpression("condition")] string? message = null)
    {
        if (condition)
            return;

        var result = FormatAssertion(new StackFrame(1, true), message);
        Warning(result);
    }

    [Conditional("DEBUG")]
    public static void Message(string message)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine($"INFO: {message}");

        Console.ForegroundColor = prevColor;
    }

    [Conditional("DEBUG")]
    public static void AssertMessage(bool condition, [CallerArgumentExpression("condition")] string? message = null)
    {
        if (condition)
            return;

        var result = FormatAssertion(new StackFrame(1, true), message);
        Message(result);
    }

    private static string FormatAssertion(StackFrame callerFrame, string? message)
    {
        StringBuilder result = new();

        result.Append("Assertion Failed");

        if (message is not null)
        {
            result.Append(": '");
            result.Append(message);
            result.Append('\'');
        }

        result.Append(" at ");

        if (callerFrame.HasMethod())
        {
            var method = callerFrame.GetMethod()!.ToString()!;
            result.Append(method[(1 + method.IndexOf(' '))..]);
        }

        if (callerFrame.HasSource())
        {
            result.Append(" in ");
            result.Append(callerFrame.GetFileName());

            result.Append(':');
            result.Append(callerFrame.GetFileLineNumber());
        }

        return result.ToString();
    }
}