using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AnalyzerTemplate
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerMagicNumAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AnalyzerMagicNum";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeMagicNum, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMagicNum(SyntaxNodeAnalysisContext context)
        {
            var classNode = (MethodDeclarationSyntax) context.Node;
            var statements = classNode.Body.Statements;
            
            foreach (var st in statements)
            {
                if (st is IfStatementSyntax ifStatement
                    && ifStatement.Condition is BinaryExpressionSyntax expressionSyntax
                    && expressionSyntax.Right is LiteralExpressionSyntax literalExpressionSyntax
                    && literalExpressionSyntax.IsKind(SyntaxKind.NumericLiteralExpression))
                {
                    var diagnostic = Diagnostic.Create(Rule, classNode.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                    return;
                }

                if (st is ForStatementSyntax forStatement
                    && forStatement.Condition is BinaryExpressionSyntax expressionForSyntax
                    && expressionForSyntax.Right is LiteralExpressionSyntax literalExpressionForSyntax
                    && literalExpressionForSyntax.IsKind(SyntaxKind.NumericLiteralExpression))
                {
                    var diagnostic = Diagnostic.Create(Rule, classNode.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                    return;
                }

                if (st is LocalDeclarationStatementSyntax declStatement
                    && declStatement.Declaration is VariableDeclarationSyntax declSyntax
                    && declSyntax.Variables.Any(variable => variable.Initializer.IsKind(SyntaxKind.NumericLiteralExpression)))
                {
                    var diagnostic = Diagnostic.Create(Rule, classNode.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                    return;
                }

                if (st is ExpressionStatementSyntax exprStatement
                    && exprStatement.Expression is InvocationExpressionSyntax invExprSyntax
                    && invExprSyntax.ArgumentList.Arguments.Any(args => args.Expression.IsKind(SyntaxKind.NumericLiteralExpression)))
                {
                    var diagnostic = Diagnostic.Create(Rule, classNode.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                    return;
                }
            }
        }
    }
}