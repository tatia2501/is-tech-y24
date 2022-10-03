using JavaParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator;

[Generator]
public class EntityGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public MemberDeclarationSyntax[] CreateFields(List<ArgDeclaration> fields)
    {
        var mem = new MemberDeclarationSyntax[fields.Count];
        int ind = 0;
        foreach (var field in fields)
        {
            mem[ind] = SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.IdentifierName(field.ArgType),
                    SyntaxFactory.Identifier(field.ArgName))
                .AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                            new AccessorDeclarationSyntax[]
                            {
                                SyntaxFactory.AccessorDeclaration(
                                        SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(SyntaxKind
                                            .SemicolonToken)),
                                SyntaxFactory.AccessorDeclaration(
                                        SyntaxKind.SetAccessorDeclaration)
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(SyntaxKind
                                            .SemicolonToken))
                            });
            ind++;
        }
        return mem;
    }

    public MemberDeclarationSyntax[] CreateClasses(EntityDeclaration[] declarations)
    {
        var mem = new MemberDeclarationSyntax[declarations.Length];
        var ind = 0;
        foreach (var dec in declarations)
        {
            mem[ind] = SyntaxFactory.ClassDeclaration(dec.EntityName)
                .AddModifiers(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddMembers(CreateFields(dec.Fields));
            ind++;
        }

        return mem;
    }
    public CompilationUnitSyntax CreateSpace(EntityDeclaration[] declarations)
    {
        var unitSyntax = SyntaxFactory.CompilationUnit()
            .AddMembers(
                    SyntaxFactory.NamespaceDeclaration(
                            SyntaxFactory.IdentifierName("MyEntities"))
                        .AddMembers(CreateClasses(declarations)))
            .NormalizeWhitespace();
        return unitSyntax;
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var petDeclaration = Parser.EntityParser(@"C:/Users/Я/Desktop/TechProg/lab-2/Pets/src/main/java/com/lab2/Pets/entities/Pet.java");
        var ownerDeclaration = Parser.EntityParser(@"C:/Users/Я/Desktop/TechProg/lab-2/Pets/src/main/java/com/lab2/Pets/entities/Owner.java");
        var declorations = new EntityDeclaration[]{petDeclaration, ownerDeclaration};
        context.AddSource("MyEntities.cs", CreateSpace(declorations).ToString());
    }
}