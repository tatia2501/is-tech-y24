using JavaParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator;

[Generator]
public class ClientGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) {}

    public static ParameterSyntax[] Args(List<ArgDeclaration> arguments)
    {
        var mem = new ParameterSyntax[arguments.Count];
        int ind = 0;
        foreach (var arg in arguments)
        {
            mem[ind]=SyntaxFactory.Parameter(
                    SyntaxFactory.Identifier(arg.ArgName))
                .WithType(
                    SyntaxFactory.IdentifierName(arg.ArgType));
            ind++;
        }
        return mem;
    }

    public static BinaryExpressionSyntax StringRequest(MethodDeclaration declaration, int ind)
    {
        if (ind > 0)
        {
            return SyntaxFactory.BinaryExpression(
                SyntaxKind.AddExpression,
                SyntaxFactory.BinaryExpression(
                    SyntaxKind.AddExpression,
                    StringRequest(declaration, ind - 1),
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal("&" + declaration.ArgList[ind].ArgName + "="))),
                SyntaxFactory.IdentifierName(declaration.ArgList[ind].ArgName));
        }
        else
        {
            return SyntaxFactory.BinaryExpression(
                SyntaxKind.AddExpression,
                SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal("http://localhost:8081/" + declaration.MethodName + "?" +
                                          declaration.ArgList[0].ArgName + "=")),
                SyntaxFactory.IdentifierName(declaration.ArgList[0].ArgName));
        }
    }

    public static ArgumentSyntax ArgumentInRequest(MethodDeclaration declaration)
    {
        if (declaration.ArgList.Count == 0)
        {
            return SyntaxFactory.Argument(
                SyntaxFactory.InterpolatedStringExpression(
                        SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                    .WithContents(
                        SyntaxFactory.SingletonList<InterpolatedStringContentSyntax>(
                            SyntaxFactory.InterpolatedStringText()
                                .WithTextToken(
                                    SyntaxFactory.Token(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.InterpolatedStringTextToken,
                                        "http://localhost:8081/" + declaration.MethodName,
                                        "http://localhost:8081/"+ declaration.MethodName,
                                        SyntaxFactory.TriviaList())))));
        }
        else
        {
            return SyntaxFactory.Argument(StringRequest(declaration, declaration.ArgList.Count - 1));
        }
    }
    public MemberDeclarationSyntax GetMethodDeclaration(MethodDeclaration declaration)
    {
        var mem =
            SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                SyntaxFactory.Identifier(declaration.Url))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword)
                    }))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(Args(declaration.ArgList))))
            .WithBody(
                SyntaxFactory.Block(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName("WebRequest"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                            SyntaxFactory.Identifier("req"))
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(
                                                SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("WebRequest"),
                                                            SyntaxFactory.IdentifierName("Create")))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(ArgumentInRequest(declaration))))))))),
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName("WebResponse"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                            SyntaxFactory.Identifier("resp"))
                                        .WithInitializer(
                                            SyntaxFactory. EqualsValueClause(
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("req"),
                                                        SyntaxFactory.IdentifierName("GetResponse")))))))),
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName("Stream"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                            SyntaxFactory.Identifier("stream"))
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("resp"),
                                                        SyntaxFactory.IdentifierName("GetResponseStream")))))))),
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName("StreamReader"))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                            SyntaxFactory.Identifier("sr"))
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(
                                                SyntaxFactory.ObjectCreationExpression(
                                                        SyntaxFactory.IdentifierName("StreamReader"))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.IdentifierName("stream")))))))))),
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                            SyntaxFactory.Identifier("Out"))
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("sr"),
                                                        SyntaxFactory.IdentifierName("ReadToEnd")))))))),
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("sr"),
                                SyntaxFactory.IdentifierName("Close")))),
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.IdentifierName("Out"))));
        return mem;
    }
    
    public MemberDeclarationSyntax PostMethodDeclaration(MethodDeclaration declaration)
    {
        var mem =
            SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    SyntaxFactory.Identifier(declaration.Url))
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        new[]
                        {
                            SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                            SyntaxFactory.Token(SyntaxKind.StaticKeyword)
                        }))
                .WithParameterList(
                    SyntaxFactory.ParameterList(
                        SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                            SyntaxFactory.Parameter(
                                    SyntaxFactory.Identifier(declaration.ArgList[0].ArgName))
                                .WithType(
                                    SyntaxFactory.IdentifierName(declaration.ArgList[0].ArgType)))))
                .WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                                .WithVariables(
                                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        SyntaxFactory.VariableDeclarator(
                                                SyntaxFactory.Identifier("data"))
                                            .WithInitializer(
                                                SyntaxFactory.EqualsValueClause(
                                                    SyntaxFactory.InvocationExpression(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName("JsonSerializer"),
                                                                SyntaxFactory.IdentifierName("Serialize")))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                                    new SyntaxNodeOrToken[]
                                                                    {
                                                                        SyntaxFactory.Argument(
                                                                            SyntaxFactory.IdentifierName(declaration.ArgList[0].ArgName)),
                                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                        SyntaxFactory.Argument(
                                                                            SyntaxFactory.TypeOfExpression(
                                                                                SyntaxFactory.IdentifierName(declaration.ArgList[0].ArgType)))
                                                                    })))))))),
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("Console"),
                                        SyntaxFactory.IdentifierName("WriteLine")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName("data")))))),
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                    SyntaxFactory.IdentifierName(
                                        SyntaxFactory.Identifier(
                                            SyntaxFactory.TriviaList(),
                                            SyntaxKind.VarKeyword,
                                            "var",
                                            "var",
                                            SyntaxFactory.TriviaList())))
                                .WithVariables(
                                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        SyntaxFactory.VariableDeclarator(
                                                SyntaxFactory.Identifier("request"))
                                            .WithInitializer(
                                                SyntaxFactory.EqualsValueClause(
                                                    SyntaxFactory.InvocationExpression(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName("WebRequest"),
                                                                SyntaxFactory.IdentifierName("Create")))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.LiteralExpression(
                                                                            SyntaxKind.StringLiteralExpression,
                                                                            SyntaxFactory.Literal(
                                                                                "http://localhost:8081/"+ declaration.Url))))))))))),
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("request"),
                                    SyntaxFactory.IdentifierName("Method")),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    SyntaxFactory.Literal("POST")))),
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("request"),
                                    SyntaxFactory.IdentifierName("ContentType")),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    SyntaxFactory.Literal("application/json")))),
                        SyntaxFactory.UsingStatement(
                                SyntaxFactory.Block(
                                    SyntaxFactory.SingletonList<StatementSyntax>(
                                        SyntaxFactory.ExpressionStatement(
                                            SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("streamWriter"),
                                                        SyntaxFactory.IdentifierName("Write")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("data")))))))))
                            .WithDeclaration(
                                SyntaxFactory.VariableDeclaration(
                                        SyntaxFactory.IdentifierName(
                                            SyntaxFactory.Identifier(
                                                SyntaxFactory.TriviaList(),
                                                SyntaxKind.VarKeyword,
                                                "var",
                                                "var",
                                                SyntaxFactory.TriviaList())))
                                    .WithVariables(
                                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            SyntaxFactory.VariableDeclarator(
                                                    SyntaxFactory.Identifier("streamWriter"))
                                                .WithInitializer(
                                                    SyntaxFactory.EqualsValueClause(
                                                        SyntaxFactory.ObjectCreationExpression(
                                                                SyntaxFactory.IdentifierName("StreamWriter"))
                                                            .WithArgumentList(
                                                                SyntaxFactory.ArgumentList(
                                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                        SyntaxFactory.Argument(
                                                                            SyntaxFactory.InvocationExpression(
                                                                                SyntaxFactory.MemberAccessExpression(
                                                                                    SyntaxKind
                                                                                        .SimpleMemberAccessExpression,
                                                                                    SyntaxFactory.IdentifierName("request"),
                                                                                    SyntaxFactory.IdentifierName(
                                                                                        "GetRequestStream")))))))))))),
                        SyntaxFactory.LocalDeclarationStatement(
                            SyntaxFactory.VariableDeclaration(
                                    SyntaxFactory.IdentifierName("WebResponse"))
                                .WithVariables(
                                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        SyntaxFactory.VariableDeclarator(
                                                SyntaxFactory.Identifier("response"))
                                            .WithInitializer(
                                                SyntaxFactory.EqualsValueClause(
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("request"),
                                                            SyntaxFactory.IdentifierName("GetResponse")))))))),
                        SyntaxFactory.UsingStatement(
                                SyntaxFactory.Block(
                                    SyntaxFactory.SingletonList<StatementSyntax>(
                                        SyntaxFactory.LocalDeclarationStatement(
                                            SyntaxFactory.VariableDeclaration(
                                                    SyntaxFactory.IdentifierName(
                                                        SyntaxFactory.Identifier(
                                                            SyntaxFactory.TriviaList(),
                                                            SyntaxKind.VarKeyword,
                                                            "var",
                                                            "var",
                                                            SyntaxFactory.TriviaList())))
                                                .WithVariables(
                                                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                        SyntaxFactory.VariableDeclarator(
                                                                SyntaxFactory.Identifier("result"))
                                                            .WithInitializer(
                                                                SyntaxFactory.EqualsValueClause(
                                                                    SyntaxFactory.InvocationExpression(
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.IdentifierName("streamReader"),
                                                                            SyntaxFactory.IdentifierName("ReadToEnd")))))))))))
                            .WithDeclaration(
                                SyntaxFactory.VariableDeclaration(
                                        SyntaxFactory.IdentifierName(
                                            SyntaxFactory.Identifier(
                                                SyntaxFactory.TriviaList(),
                                                SyntaxKind.VarKeyword,
                                                "var",
                                                "var",
                                                SyntaxFactory.TriviaList())))
                                    .WithVariables(
                                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            SyntaxFactory.VariableDeclarator(
                                                    SyntaxFactory.Identifier("streamReader"))
                                                .WithInitializer(
                                                    SyntaxFactory.EqualsValueClause(
                                                        SyntaxFactory.ObjectCreationExpression(
                                                                SyntaxFactory.IdentifierName("StreamReader"))
                                                            .WithArgumentList(
                                                                SyntaxFactory.ArgumentList(
                                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                        SyntaxFactory.Argument(
                                                                            SyntaxFactory.InvocationExpression(
                                                                                SyntaxFactory.MemberAccessExpression(
                                                                                    SyntaxKind
                                                                                        .SimpleMemberAccessExpression,
                                                                                    SyntaxFactory.IdentifierName("response"),
                                                                                    SyntaxFactory.IdentifierName(
                                                                                        "GetResponseStream")))))))))))),
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("response"),
                                    SyntaxFactory.IdentifierName("Close"))))));
        return mem;
    }
    public MemberDeclarationSyntax[] CreateMethods(GeneratorExecutionContext context, List<MethodDeclaration> declaration)
    {
        var mem = new MemberDeclarationSyntax[declaration.Count];
        int ind = 0;
        foreach (var dec in declaration)
        {
            if (dec.HttpMethodName == "get")
            {
                mem[ind] = GetMethodDeclaration(dec);
            }
            else
            {
                mem[ind] = PostMethodDeclaration(dec);
            }

            ind++;
        }

        return mem;
    }
    public CompilationUnitSyntax CreateSource(GeneratorExecutionContext context, List<MethodDeclaration> declaration)
    {
        var mem =SyntaxFactory.CompilationUnit()
            .WithUsings(
                SyntaxFactory.List<UsingDirectiveSyntax>(
                    new UsingDirectiveSyntax[]
                    {
                        SyntaxFactory.UsingDirective(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.IdentifierName("System"),
                                SyntaxFactory.IdentifierName("Net"))),
                        SyntaxFactory.UsingDirective(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.QualifiedName(
                                    SyntaxFactory.IdentifierName("System"),
                                    SyntaxFactory.IdentifierName("Text")),
                                SyntaxFactory.IdentifierName("Json"))),
                        SyntaxFactory.UsingDirective(
                            SyntaxFactory.IdentifierName("MyEntities"))
                    }))
            .WithMembers(
                SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                    SyntaxFactory.NamespaceDeclaration(
                            SyntaxFactory.IdentifierName("MyClient"))
                        .WithMembers(
                            SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                                SyntaxFactory.ClassDeclaration("Client")
                                    .WithModifiers(
                                        SyntaxFactory.TokenList(
                                            SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                                    .AddMembers(CreateMethods(context, declaration))
                                ))))
            .NormalizeWhitespace();
        return mem;
    }

    public void Execute(GeneratorExecutionContext context)
    {
        List<MethodDeclaration> methodOwnerDeclaration = Parser.ControllerParser("C:/Users/Я/Desktop/TechProg/lab-2/Pets/src/main/java/com/lab2/Pets/controllers/OwnerController.java");
        List<MethodDeclaration> methodPetDeclaration = Parser.ControllerParser("C:/Users/Я/Desktop/TechProg/lab-2/Pets/src/main/java/com/lab2/Pets/controllers/PetController.java");
        foreach (var dec in methodPetDeclaration)
        {
            methodOwnerDeclaration.Add(dec);
        }
        context.AddSource("MyClient.cs", CreateSource(context, methodOwnerDeclaration).ToString());
    }
}