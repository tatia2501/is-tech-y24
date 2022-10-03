using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerEqualsAnalyzer,
    AnalyzerTemplate.AnalyzerEqualsCodeFixProvider>;

namespace AnalyzerTemplate.Test
{
    [TestClass]
    public class AnalyzerEqualsTests
    {
        [TestMethod]
        public async Task SimpleChangeTest()
        {
            var code = @"namespace TestSpace
{
    class Test
    {
        public int Test1()
        {
            int a = 2;
            int b = 5;
            if (a == b)
            {
            }
        }

        static void Main()
        {
        }
    }
}";

            var expectedChangedCode = @"namespace TestSpace
{
    class Test
    {
        public int Test1()
        {
            int a = 2;
            int b = 5;
            if (Equals(a, b))
            {
            }
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await UtilitiesForEquals.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerEqualsCodeFixProvider();

            CodeAction registeredCodeAction = null;

            var context = new CodeFixContext(document, diagnostic, (codeAction, _) =>
            {
                if (registeredCodeAction != null)
                    throw new Exception("Code action was registered more than once");

                registeredCodeAction = codeAction;

            }, CancellationToken.None);

            await codeFixProvider.RegisterCodeFixesAsync(context);

            if (registeredCodeAction == null)
                throw new Exception("Code action was not registered");

            var operations = await registeredCodeAction.GetOperationsAsync(CancellationToken.None);

            foreach(var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);
            
            var newCode = (await updatedDocument.GetTextAsync()).ToString();
            
            // for (int i = 0; i < 100; i++)
            // {
            //     Console.Write(i + " ");
            //     Console.WriteLine("__ch:__" + newCode[i] + "__chAct:__" + expectedChangedCode[i]);
            // }
            
            // Assert.AreEqual(expectedChangedCode.Replace(" ", ""), newCode.Replace(" ", ""));
            Assert.AreEqual(expectedChangedCode, newCode);

        }
        
        [TestMethod]
        public async Task NoChangeTest()
        {
            var code = @"
namespace TestSpace
{
    class Test
    {
        public int Test1()
        {
            int a = 2;
            int b = 5;
            if (a > b)
            {
            }
        }

        static void Main()
        {
        }
    }
}";

            var expectedChangedCode = @"
namespace TestSpace
{
    class Test
    {
        public int Test1()
        {
            int a = 2;
            int b = 5;
            if (a > b)
            {
            }
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await UtilitiesForEquals.GetDiagnosticsAdvanced(code);       
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerEqualsCodeFixProvider();

            CodeAction registeredCodeAction = null;

            var context = new CodeFixContext(document, diagnostic, (codeAction, _) =>
            {
                if (registeredCodeAction != null)
                    throw new Exception("Code action was registered more than once");

                registeredCodeAction = codeAction;

            }, CancellationToken.None);

            await codeFixProvider.RegisterCodeFixesAsync(context);

            if (registeredCodeAction == null)
                throw new Exception("Code action was not registered");

            var operations = await registeredCodeAction.GetOperationsAsync(CancellationToken.None);

            foreach(var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task RedefinedOperatorTest()
        {
            var code = @"namespace TestSpace
{
    class Test
    {
        public int Test1()
        {
            Tags a = new Tags();
            Tags b = new Tags();
            if (a == b)
            {
            }
        }

        static void Main()
        {
        }
    }

    class Tags
    {
        int mask;

        public static bool operator !=(Tags x, Tags y)
        {
            return !(x == y);
        }

        public static bool operator ==(Tags x, Tags y)
        {
            return x.mask == y.mask;
        }    
    }
}";

            var expectedChangedCode = @"namespace TestSpace
{
    class Test
    {
        public int Test1()
        {
            Tags a = new Tags();
            Tags b = new Tags();
            if (a == b)
            {
            }
        }

        static void Main()
        {
        }
    }

    class Tags
    {
        int mask;

        public static bool operator !=(Tags x, Tags y)
        {
            return !(x == y);
        }

        public static bool operator ==(Tags x, Tags y)
        {
            return x.mask == y.mask;
        }    
    }
}";

            var (diagnostics, document, workspace) = await UtilitiesForEquals.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerEqualsCodeFixProvider();

            CodeAction registeredCodeAction = null;

            var context = new CodeFixContext(document, diagnostic, (codeAction, _) =>
            {
                if (registeredCodeAction != null)
                    throw new Exception("Code action was registered more than once");

                registeredCodeAction = codeAction;

            }, CancellationToken.None);

            await codeFixProvider.RegisterCodeFixesAsync(context);

            if (registeredCodeAction == null)
                throw new Exception("Code action was not registered");

            var operations = await registeredCodeAction.GetOperationsAsync(CancellationToken.None);

            foreach(var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);
            
            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task WithInterfaceTest()
        {
            var code = @"namespace TestSpace
{
    class Test
    {
        public int Test1()
        {
            int a = 2;
            if (a == IMovable.maxSpeed)
            {
            }
        }

        static void Main()
        {
        }
    }
    
    interface IMovable
    {
    public const int minSpeed = 0;
    public static int maxSpeed = 60;
    public void Move();
    protected internal string Name { get; set; } 
    }
}";

            var expectedChangedCode = @"namespace TestSpace
{
    class Test
    {
        public int Test1()
        {
            int a = 2;
            if (Equals(a, IMovable.maxSpeed))
            {
            }
        }

        static void Main()
        {
        }
    }
    
    interface IMovable
    {
    public const int minSpeed = 0;
    public static int maxSpeed = 60;
    public void Move();
    protected internal string Name { get; set; } 
    }
}";

            var (diagnostics, document, workspace) = await UtilitiesForEquals.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerEqualsCodeFixProvider();

            CodeAction registeredCodeAction = null;

            var context = new CodeFixContext(document, diagnostic, (codeAction, _) =>
            {
                if (registeredCodeAction != null)
                    throw new Exception("Code action was registered more than once");

                registeredCodeAction = codeAction;

            }, CancellationToken.None);

            await codeFixProvider.RegisterCodeFixesAsync(context);

            if (registeredCodeAction == null)
                throw new Exception("Code action was not registered");

            var operations = await registeredCodeAction.GetOperationsAsync(CancellationToken.None);

            foreach(var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);
            
            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
    }
}