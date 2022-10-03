using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace AnalyzerTemplate
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AnalyzerMagicNumCodeFixProvider)), Shared]
    public class AnalyzerMagicNumCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerMagicNumAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();
    
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedDocument: c => MakeMagicNum(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }

        private async Task<Document> MakeMagicNum(Document document, ClassDeclarationSyntax declarationSyntax,
            CancellationToken cancellationToken)
        {
            var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var root = await tree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
            var members = declarationSyntax.Members;
            int magicNumCount = 1;
            string name = "";
            int value = 0;
            bool flag = true;
            List<int> values = new List<int>();
            foreach(var mem in members)
            {
                if (mem is FieldDeclarationSyntax decl
                    && decl.Modifiers.Any(SyntaxKind.ConstKeyword))
                {
                    magicNumCount++;
                    foreach (var variable in decl.Declaration.Variables)
                    {
                        values.Add((int) variable.Initializer.Value.GetFirstToken().Value);
                    }
                }
                
                if (mem is MethodDeclarationSyntax method)
                {
                    var statements = method.Body.Statements;
                    foreach (var st in statements)
                    {
                        if (st is IfStatementSyntax ifStatement
                            && ifStatement.Condition is BinaryExpressionSyntax expressionSyntax
                            && expressionSyntax.Right is LiteralExpressionSyntax literalExpressionSyntax
                            && literalExpressionSyntax.IsKind(SyntaxKind.NumericLiteralExpression))
                        {
                            value = (int)literalExpressionSyntax.Token.Value;
                            for (int j = 0; j < values.Count; j++)
                            {
                                if (value == values[j])
                                {
                                    name = "MagicNumber" + (j+1).ToString();
                                    flag = false;
                                    break;
                                }
                                else name = "MagicNumber" + magicNumCount.ToString();
                            }
                            
                            var newExpression = SyntaxFactory.BinaryExpression(
                                expressionSyntax.Kind(),
                                    expressionSyntax.Left,
                                    SyntaxFactory.IdentifierName(name));
                            root = root.ReplaceNode(expressionSyntax, newExpression).NormalizeWhitespace();
                        }
                        
                        if (st is ForStatementSyntax forStatement
                            && forStatement.Condition is BinaryExpressionSyntax expressionForSyntax
                            && expressionForSyntax.Right is LiteralExpressionSyntax literalExpressionForSyntax
                            && literalExpressionForSyntax.IsKind(SyntaxKind.NumericLiteralExpression))
                        {
                            value = (int)literalExpressionForSyntax.Token.Value;
                            for (int j = 0; j < values.Count; j++)
                            {
                                if (value == values[j])
                                {
                                    name = "MagicNumber" + j.ToString();
                                    flag = false;
                                    break;
                                }
                                else name = "MagicNumber" + magicNumCount.ToString();
                            }

                            var newExpression = SyntaxFactory.BinaryExpression(
                                expressionForSyntax.Kind(),
                                expressionForSyntax.Left,
                                SyntaxFactory.IdentifierName(name));
                            root = root.ReplaceNode(expressionForSyntax, newExpression).NormalizeWhitespace();
                        }
                        
                        if (st is LocalDeclarationStatementSyntax declStatement
                            && declStatement.Declaration is VariableDeclarationSyntax declSyntax)
                        {
                            foreach (var variable in declSyntax.Variables)
                            {
                                if (variable.Initializer.Value is LiteralExpressionSyntax litExpressionForSyntax
                                    && litExpressionForSyntax.IsKind(SyntaxKind.NumericLiteralExpression))
                                {
                                    value = (int) litExpressionForSyntax.Token.Value;
                                    for (int j = 0; j < values.Count; j++)
                                    {
                                        if (value == values[j])
                                        {
                                            name = "MagicNumber" + (j+1).ToString();
                                            flag = false;
                                            break;
                                        }
                                        else name = "MagicNumber" + magicNumCount.ToString();
                                    }

                                    var newInitializer = SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.IdentifierName(name));
                                    root = root.ReplaceNode(variable.Initializer, newInitializer).NormalizeWhitespace();
                                }
                            }
                        }

                        if (st is ExpressionStatementSyntax exprStatement
                            && exprStatement.Expression is InvocationExpressionSyntax invExprSyntax)
                        {
                            foreach (var arg in invExprSyntax.ArgumentList.Arguments)
                            {
                                if (arg.Expression.IsKind(SyntaxKind.NumericLiteralExpression))
                                {
                                    value = (int) arg.Expression.GetFirstToken().Value;
                                    for (int j = 0; j < values.Count; j++)
                                    {
                                        if (value == values[j])
                                        {
                                            name = "MagicNumber" + (j+1).ToString();
                                            flag = false;
                                            break;
                                        }
                                        else name = "MagicNumber" + magicNumCount.ToString();
                                    }
                                    
                                    var newArg = SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(name));
                                    root = root.ReplaceNode(arg, newArg).NormalizeWhitespace();
                                }
                            }
                        }
                    }
                }
            }

            if (flag)
            {
                SyntaxTree newTree = CSharpSyntaxTree.ParseText(root.ToString());
                CompilationUnitSyntax newRoot = await newTree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
                
                var newFieldDeclaration = SyntaxFactory.FieldDeclaration(
                        SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.IntKeyword)))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                            SyntaxFactory.Identifier(name))
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(value)))))))
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            new []{
                                SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                                SyntaxFactory.Token(SyntaxKind.ConstKeyword)}));

                var namespaceDecl = (NamespaceDeclarationSyntax) newRoot.Members.First();
                var classDecl = (ClassDeclarationSyntax) namespaceDecl.Members.First();
                classDecl = classDecl.WithMembers(classDecl.Members.Insert(1, newFieldDeclaration));
                namespaceDecl = namespaceDecl.WithMembers(new SyntaxList<MemberDeclarationSyntax>(classDecl));

                newRoot = newRoot.WithMembers(new SyntaxList<MemberDeclarationSyntax>(namespaceDecl)).NormalizeWhitespace();
            
                return document.WithSyntaxRoot(newRoot);
            }
            else return document.WithSyntaxRoot(root);
        }
    }
}