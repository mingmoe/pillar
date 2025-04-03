﻿using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PillarOfPillar;

[Generator]
public class EventGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
                              .CreateSyntaxProvider(
                                  (s, _) => s is FieldDeclarationSyntax,
                                  (ctx, _) => GetClassDeclarationForSourceGen(ctx))
                              .Where(t => t.attributeFound)
                              .Select((t, _) => t.Item1);

        context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
                                     ((ctx, t) => GenerateCode(ctx, t.Left, t.Right)));
    }

    /// <summary>
    /// Checks whether the Node is annotated with the [Report] attribute and maps syntax context to the specific node type (ClassDeclarationSyntax).
    /// </summary>
    /// <param name="context">Syntax context, based on CreateSyntaxProvider predicate</param>
    /// <returns>The specific cast and whether the attribute was found.</returns>
    private static (FieldDeclarationSyntax, bool attributeFound) GetClassDeclarationForSourceGen(
        GeneratorSyntaxContext context)
    {
        var fieldDeclarationSyntax = (FieldDeclarationSyntax)context.Node;

        // Go through all attributes of the class.
        foreach (AttributeListSyntax attributeListSyntax in fieldDeclarationSyntax.AttributeLists)
        foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                continue;

            string attributeName = attributeSymbol.ContainingType.ToDisplayString();

            // Check the full name of the [EmitEventAttribute] attribute.
            if (attributeName == $"Pillar.EmitEventAttribute")
                return (fieldDeclarationSyntax, true);
        }

        return (fieldDeclarationSyntax, false);
    }

    /// <summary>
    /// Generate code action.
    /// It will be executed on specific nodes (ClassDeclarationSyntax annotated with the [Report] attribute) changed by the user.
    /// </summary>
    /// <param name="context">Source generation context used to add source files.</param>
    /// <param name="compilation">Compilation used to provide access to the Semantic Model.</param>
    /// <param name="fieldDeclarationSyntaxes">Nodes annotated with the [Report] attribute that trigger the generate action.</param>
    private void GenerateCode(SourceProductionContext context, Compilation compilation,
                              ImmutableArray<FieldDeclarationSyntax> fieldDeclarationSyntaxes)
    {
        try
        {
            context.Debug("ENTER THE CONTEXT");
            context.Debug("COUNT:" + fieldDeclarationSyntaxes.Length);
            // Go through all filtered class declarations.
            foreach (var fieldDeclarationSyntax in fieldDeclarationSyntaxes)
            {
                // We need to get semantic model of the class to retrieve metadata.
                var semanticModel = compilation.GetSemanticModel(fieldDeclarationSyntax.SyntaxTree);

                // Symbols allow us to get the compile-time information.
                if (semanticModel.GetDeclaredSymbol(fieldDeclarationSyntax.Parent!) is not INamedTypeSymbol fieldSymbol)
                {
                    context.Debug("FAILED TO GET SYMBOL");
                    continue;
                }

                var namespaceName = fieldSymbol.ContainingNamespace.ToDisplayString();

                // 'Identifier' means the token of the node. Get class name from the syntax node.
                var variable = fieldDeclarationSyntax.Declaration;

                var type = (GenericNameSyntax)variable.ChildNodes().Where((node => node is GenericNameSyntax))
                                                      .First();

                var typeName = semanticModel
                               .GetSymbolInfo(type.TypeArgumentList.ChildNodes()
                                                  .Where(node => node is IdentifierNameSyntax)!.First()).Symbol!;

                foreach (var rawDeclarator in variable.ChildNodes().Where(node => node is VariableDeclaratorSyntax))
                {
                    var declarator = ((VariableDeclaratorSyntax)rawDeclarator);
                    var fieldName = declarator.Identifier.Text;

                    context.Debug("GET Declarator:" + fieldName);

                    var fixname = fieldName.TrimStart('_');

                    var eventName = $"{char.ToUpperInvariant(fixname[0])}{fixname.Substring(1)}";

                    var className = ((ClassDeclarationSyntax)fieldDeclarationSyntax.Parent!).Identifier.Text;

                    // Build up the source code
                    var code = $$"""
                                 // <auto-generated/>

                                 namespace {{namespaceName}};

                                 public partial class {{className}}
                                 {
                                     public event EventHandler<{{typeName!.ToDisplayString()}}> {{eventName}} {
                                 		add{
                                 			{{fieldName}}.Register(value);
                                 		}
                                 		remove{
                                 			{{fieldName}}.Unregister(value);
                                 		}
                                 	}
                                 }

                                 """;

                    // Add the source code to the compilation.
                    context.AddSource($"{namespaceName}.{className}.{eventName}.event.g.cs",
                                      SourceText.From(code, Encoding.UTF8));
                }
            }
        }
        catch (System.Exception ex)
        {
            context.Debug(ex.ToString().Replace("\n",";;;"));
        }
    }
}