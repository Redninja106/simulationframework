using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

internal static class Debug
{
    public static TraceFlags ActiveTraceFlags = default;

    private static TextWriter? redirectWriter;


    public static void Trace(TraceFlags category, string message)
    {
        if (ActiveTraceFlags.HasFlag(category))
        {
            WriteOut($"Trace ({category}): {message}\n", ConsoleColor.DarkGray);
        }
    }

    public static void Message(string message)
    {
        var stackFrame = new StackFrame(1);

        var caller = stackFrame.GetMethod();

        WriteOut($"{caller!.DeclaringType!.Name}: {message}\n", ConsoleColor.Gray);
    }

    public static void Warn(string message)
    {
        WriteOut($"Warning: {message}\n", ConsoleColor.Yellow);
    }

    public static void Assert(bool condition, [CallerArgumentExpression("condition")] string? message = null)
    {
        if (condition)
            return;

        var errorMessage = FormatAssertion(new StackFrame(1, true), message);

        throw new SimulationFrameworkException(errorMessage);
    }

    public static void Assert(bool condition, Exception exception)
    {
        if (condition)
            return;

        throw exception;
    }

    public static void Assert<TException>(bool condition) where TException : Exception, new()
    {
        if (condition)
            return;

        throw new TException();
    }

    public static void Redirect(TextWriter? writer)
    {
        redirectWriter = writer;
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

        result.Append('\n');

        return result.ToString();
    }

    private static void WriteOut(string message, ConsoleColor color)
    {
        if (redirectWriter is not null)
        {
            redirectWriter.Write(message);
        }
        else
        {
            (color, Console.ForegroundColor) = (Console.ForegroundColor, color);

            Console.Write(message);

            (Console.ForegroundColor, _) = (color, Console.ForegroundColor);
        }
    }

    [Flags]
    public enum TraceFlags
    {
        None,
        EventDispatcher = 1 << 0,
    }
}