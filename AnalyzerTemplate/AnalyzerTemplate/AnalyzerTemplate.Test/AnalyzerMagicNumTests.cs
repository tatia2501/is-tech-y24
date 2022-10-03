using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerMagicNumAnalyzer,
    AnalyzerTemplate.AnalyzerMagicNumCodeFixProvider>;

namespace AnalyzerTemplate.Test
{
    [TestClass]
    public class AnalyzerMagicNumTest
    {
        [TestMethod]
        public async Task IfStatementTest()
        {
            var code = @"namespace TestSpace
{
    public class Test
    {
        public const int MagicNumber1 = 3;
        public void Test1()
        {
            var a = MagicNumber1;
            if (a >= 8)
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
    public class Test
    {
        public const int MagicNumber1 = 3;
        public const int MagicNumber2 = 8;
        public void Test1()
        {
            var a = MagicNumber1;
            if (a >= MagicNumber2)
            {
            }
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await UtilitiesForMagicNum.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerMagicNumCodeFixProvider();

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

            foreach (var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task LocalDeclarationTest()
        {
            var code = @"namespace TestSpace{
    public class Test
    {
        public const int MagicNumber1 = 3;
        public void Test2()
        {
            var a = 4;
        }

        static void Main()
        {
        }
    }
}";

            var expectedChangedCode = @"namespace TestSpace{
    public class Test
    {
        public const int MagicNumber1 = 3;
        public const int MagicNumber2 = 4;
        public void Test2()
        {
            var a = MagicNumber2;
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await UtilitiesForMagicNum.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerMagicNumCodeFixProvider();

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

            foreach (var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);
            
            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task ExpressionStatementTest()
        {
            var code = @"namespace TestSpace
{
    public class Test
    {
        public const int MagicNumber1 = 3;
        public void TestVoid(int a, int b)
        {
        }

        public void Test1()
        {
            TestVoid(MagicNumber1, 10);
        }

        static void Main()
        {
        }
    }
}";

            var expectedChangedCode = @"namespace TestSpace
{
    public class Test
    {
        public const int MagicNumber1 = 3;
        public const int MagicNumber2 = 10;
        public void TestVoid(int a, int b)
        {
        }

        public void Test1()
        {
            TestVoid(MagicNumber1, MagicNumber2);
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await UtilitiesForMagicNum.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerMagicNumCodeFixProvider();

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

            foreach (var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task ExistingMagicNumTest()
        {
            var code = @"namespace TestSpace
{
    public class Test
    {
        public const int MagicNumber1 = 3;
        public const int MagicNumber2 = 5;
        public const int MagicNumber3 = 7;
        public void Test1()
        {
            var a = MagicNumber1;
            if (a >= 5)
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
    public class Test
    {
        public const int MagicNumber1 = 3;
        public const int MagicNumber2 = 5;
        public const int MagicNumber3 = 7;
        public void Test1()
        {
            var a = MagicNumber1;
            if (a >= MagicNumber2)
            {
            }
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await UtilitiesForMagicNum.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerMagicNumCodeFixProvider();

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

            foreach (var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
    }
}