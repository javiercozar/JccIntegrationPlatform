using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Jcc.Commons.Platform.Generators;

/// <summary>
/// Incremental Source Generator que detecta automáticamente clases que implementan
/// IQueryHandler&lt;TRequest, TResponse&gt; y genera el código de registro en el
/// inyector de dependencias.
/// </summary>
[Generator]
public class QueryHandlerSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Paso 1: Detectar todas las clases que implementan IQueryHandler
        var queryHandlerClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => IsQueryHandlerCandidate(node),
                transform: static (context, _) => ExtractQueryHandlerInfo(context)
            )
            .Where(static info => info is not null)
            .Collect();

        // Paso 2: Generar el código del DependencyInjectionConfig
            context.RegisterSourceOutput(queryHandlerClasses, static (context, handlers) =>
            {
                var generatedCode = QueryHandlerCodeGenerator.GenerateDependencyInjectionCode(
                    handlers.Cast<QueryHandlerInfo>().ToImmutableArray()
                );

                context.AddSource("DependencyInjectionConfig.QueryHandlers.g.cs", generatedCode);
            });
    }

    /// <summary>
    /// Determina si un nodo de sintaxis es un candidato para ser un manejador de consultas.
    /// </summary>
    private static bool IsQueryHandlerCandidate(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax classDeclaration
            && classDeclaration.BaseList is not null
            && classDeclaration.BaseList.Types.Any(baseType =>
                baseType.Type.ToString().StartsWith("IQueryHandler"));
    }

    /// <summary>
    /// Extrae la información del manejador de consultas desde el contexto.
    /// </summary>
    private static QueryHandlerInfo? ExtractQueryHandlerInfo(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration)
            return null;

        // Obtener el símbolo de la clase
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
        if (symbol is null || symbol.IsAbstract)
            return null;

        // Buscar la interfaz IQueryHandler implementada
        var queryHandlerInterface = symbol.AllInterfaces
            .FirstOrDefault(i => i.Name == "IQueryHandler" && i.TypeArguments.Length == 2);

        if (queryHandlerInterface is null)
            return null;

        var requestType = queryHandlerInterface.TypeArguments[0];
        var responseType = queryHandlerInterface.TypeArguments[1];

        return new QueryHandlerInfo(
            ClassName: symbol.Name,
            FullClassName: symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Namespace: symbol.ContainingNamespace.ToDisplayString(),
            RequestType: requestType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            ResponseType: responseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            InterfaceFullName: queryHandlerInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
        );
    }
}

/// <summary>
/// Información extraída de un manejador de consultas.
/// </summary>
internal record QueryHandlerInfo(
    string ClassName,
    string FullClassName,
    string Namespace,
    string RequestType,
    string ResponseType,
    string InterfaceFullName
);