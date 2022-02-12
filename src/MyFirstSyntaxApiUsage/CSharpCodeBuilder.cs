using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace MyFirstSyntaxApiUsage;
public class CSharpCodeBuilder
{
    public Assembly CompileCode(string sourceCode, params Type[] types)
    {
        var root = typeof(Enumerable).GetTypeInfo().Assembly.Location;
        var coreDir = Directory.GetParent(root);

        var paths = types?.Select(x => x.GetTypeInfo().Assembly.Location).Distinct().ToList() ?? new List<string>();

        paths.Add(typeof(Object).GetTypeInfo().Assembly.Location);
        paths.Add(coreDir.FullName + Path.DirectorySeparatorChar + "mscorlib.dll");
        paths.Add(coreDir.FullName + Path.DirectorySeparatorChar + "System.Runtime.dll");

        var compilation = CSharpCompilation.Create(Guid.NewGuid() + ".dll")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(paths.Select(path => MetadataReference.CreateFromFile(path)))
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));

        var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            throw new Exception("Compilation failed");
        }

        var assembly = Assembly.Load(ms.ToArray());
        return assembly;
    }

    public TObject? CreateInstance<TObject>(Assembly ass,
        object[]? constructorArgs = null,
        int index = 0)
    {
        var type = ass.ExportedTypes.ElementAtOrDefault(index);
        if (type is null) return default(TObject?);
        return (TObject?)Activator.CreateInstance(type);
    }

}
