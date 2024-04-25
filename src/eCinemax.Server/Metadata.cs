using System.Reflection;

namespace eCinemax.Server;

public static class Metadata
{
    public static readonly Assembly Assembly = typeof(Program).Assembly;

    public static readonly string Name = Assembly.GetName().Name!;
}