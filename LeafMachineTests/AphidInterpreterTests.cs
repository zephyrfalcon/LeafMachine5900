using System.Collections.Generic;  // avoid collision with NUnit.Framework.List
using NUnit.Framework;
using LeafMachine.Aphid;
using NuGet.Frameworks;
using LeafMachine.Aphid.Types;

namespace LeafMachineTests
{
    public class AphidInterpreterTests
    {
        AphidInterpreter aip;

        [SetUp]
        public void Setup()
        {
            this.aip = new AphidInterpreter();
        }

        [Test]
        public void TestPushingSimple()
        {
            aip.Run("1 2 3");
            Assert.AreEqual(aip.stack.Size(), 3);
            Assert.AreEqual(aip.stack.SimpleRepr(), "1 2 3");
        }

        [Test]
        public void TestDup()
        {
            aip.Run("1 2 dup");
            Assert.AreEqual(aip.stack.SimpleRepr(), "1 2 2");
        }

        [Test]
        public void TestBrackets()
        {
            aip.Run("1 2 [ 3");
            // at this point, only the "top stack" is visible, and it only has a 3
            Assert.AreEqual(aip.stack.SimpleRepr(), "3");
            aip.Run("4 ] 5");
            Assert.AreEqual(aip.stack.SimpleRepr(), "1 2 [ 3 4 ] 5");
            AphidType x1 = aip.stack.Pop();
            Assert.IsTrue(x1 is AphidInteger);
            Assert.AreEqual(x1.ToString(), "5");
            AphidType x2 = aip.stack.TOS();
            Assert.IsTrue(x2 is AphidList);
            Assert.AreEqual(x2.ToString(), "[ 3 4 ]");
        }

        [Test]
        public void TestBlocks()
        {
            aip.Run("1 2 { dup } exec");
            Assert.AreEqual(aip.stack.SimpleRepr(), "1 2 2");
        }

        [Test]
        public void TestNull()
        {
            aip.Run("null");
            Assert.IsTrue(aip.stack.TOS() is AphidNull);
        }

        [Test]
        public void TestVariables()
        {
            aip.Run("42 :foo setvar");
            Assert.AreEqual(aip.GetVar("foo").ToString(), "42");
            aip.Run(":foo getvar");
            Assert.AreEqual(aip.stack.TOS().ToString(), "42");
        }
    }

    public class StackWordTests {
        // tests for words that simply manipulate the stack and don't do much/anything else.
        // we can test this by providing the code to run and then inspecting what the stack
        // looks like afterwards, using Stack.SimpleRepr().
        AphidInterpreter aip;

        [SetUp]
        public void Setup()
        {
            this.aip = new AphidInterpreter();
        }

        public void Check(string code, string stackRepr)
        {
            aip.Run(code);
            Assert.AreEqual(stackRepr, aip.stack.SimpleRepr());
        }

        [Test]
        public void TestDup()
        {
            Check("1 2 dup", "1 2 2");        }

        [Test]
        public void TestDrop()
        {
            Check("1 2 3 drop", "1 2");
        }

        [Test]
        public void TestSwap()
        {
            Check("4 10 swap", "10 4");
        }

        [Test]
        public void TestBrackets()
        {
            Check("1 2 [ 4 5 ]", "1 2 [ 4 5 ]");
        }

        [Test]
        public void TestBraces()
        {
            Check("1 2 { 3 4 }", "1 2 { 3 4 }");
        }

        [Test]
        public void TestNestedBlocks()
        {
            Check("1 { 2 3 { dup } 4 } 5", "1 { 2 3 { dup } 4 } 5");
        }

        [Test]
        public void TestStrToChars()
        {
            Check("\"abc\" str>chars", "[ \"a\" \"b\" \"c\" ]");
        }

        [Test]
        public void TestNull()
        {
            Check("null", "null");
        }

        [Test]
        public void TestPlus()
        {
            Check("1 2 +", "3");
        }

    }
}