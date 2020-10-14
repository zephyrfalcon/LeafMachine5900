using System.Collections.Generic;  // avoid collision with NUnit.Framework.List
using NUnit.Framework;
using LeafMachine.Aphid;
using NuGet.Frameworks;

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
            Check("1 2 dup", "1 2 2");
        }

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
    }
}