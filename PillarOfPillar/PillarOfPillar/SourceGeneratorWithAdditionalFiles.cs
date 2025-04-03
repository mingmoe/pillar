using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis;

namespace PillarOfPillar;

/// <summary>
/// A sample source generator that creates C# classes based on the text file (in this case, Domain Driven Design ubiquitous language registry).
/// When using a simple text file as a baseline, we can create a non-incremental source generator.
/// </summary>
public class SourceGeneratorWithAdditionalFiles : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.AdditionalTextsProvider
                              .Where(f => Path.GetFileName(f.Path) == "DDD.UbiquitousLanguageRegistry.txt")
                              .Collect();

        context.RegisterSourceOutput(provider, GenerateCode);
    }

    private void GenerateCode(SourceProductionContext context, ImmutableArray<AdditionalText> files)
    {
        foreach (var file in files)
        {
            // Get the text of the file.
            var lines = file.GetText(context.CancellationToken)?.ToString().Split('\n');
            if (lines == null)
                continue;

            foreach (var line in lines)
            {
                var className = line.Trim();

                // Build up the source code.
                string source = $@"// <auto-generated/>

namespace Entities
{{
    public partial class {className}
    {{
    }}
}}
";
                // Add the source code to the compilation.
                context.AddSource($"{className}.g.cs", source);
            }
        }
    }
}