using Microsoft.CodeAnalysis;

namespace PillarOfPillar;

internal static class Helper
{
    public static void Debug(this GeneratorExecutionContext context, string msg)
    {
        context.ReportDiagnostic(Diagnostic.Create(
                                     new DiagnosticDescriptor("DEBUG0", "SOURCE_GENERATOR_DEBUG_OUTPUT", "{0}", "", DiagnosticSeverity.Warning, true),
                                     null,
                                     msg));
    }

    public static void Debug(this SourceProductionContext context, string msg)
    {
        context.ReportDiagnostic(Diagnostic.Create(
                                     new DiagnosticDescriptor("DEBUG0", "SOURCE_GENERATOR_DEBUG_OUTPUT", "{0}", "", DiagnosticSeverity.Warning, true),
                                     null,
                                     msg));
    }

    public class A : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {

            throw new System.NotImplementedException();
        }

        public void Initialize(GeneratorInitializationContext context)
        {

            throw new System.NotImplementedException();
        }
    }
}
