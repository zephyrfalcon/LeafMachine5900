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
        public void TestOver()
        {
            Check("1 2 over", "1 2 1");
        }

        [Test]
        public void TestRol()
        {
            Check("1 2 3 rol", "2 3 1");
        }

        [Test]
        public void TestRor()
        {
            Check("1 2 3 ror", "3 1 2");
        }

        [Test]
        public void TestNip()
        {
            Check("1 2 3 nip", "1 3");
        }

        [Test]
        public void TestTuck()
        {
            Check("1 2 tuck", "2 1 2");
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
            Check("-3 +", "0");  // 3 is still on the stack
        }

        [Test]
        public void TestForEach()
        {
            Check("[ 4 5 6 ] { } for-each", "4 5 6");  // just put the items on the stack
            aip.stack.Clear();
            Check("[ 1 2 3 ] { 1 + } for-each", "2 3 4"); // ditto but add 1 to them first
        }

        [Test]
        public void TestPack()
        {
            Check("0 10 11 12 3 pack", "0 [ 10 11 12 ]");
            aip.stack.Clear();
            Check("1 2 3 0 pack", "1 2 3 [ ]");  // `0 pack` simply produces an empty list and leave the rest of the stack untouched
            aip.stack.Clear();
            Check("1 2 3 -1 pack", "1 2 3 [ ]"); // same for a negative number
        }

        [Test]
        public void TestUnpack()
        {
            Check("[ 1 2 3 ] unpack", "1 2 3");
        }

        [Test]
        public void TestThreeRev()
        {
            Check("1 2 3 4 3rev", "1 4 3 2");
        }

        [Test]
        public void TestFourRev()
        {
            Check(":a :b :c :d 4rev", "d c b a");  // FIXME?
            // There's no bug here... a symbol literal like :a is Execute()d, which strips the
            // leading colon and puts that symbol (minus the colon) on the stack. So when we
            // display it *as data on the stack*, it does not have a colon. It's not code, that's
            // why it differs from displaying a block, which *does* contain code and therefore
            // may also contain un-Execute()d symbol literals.
        }

        [Test]
        public void TestRev()
        {
            Check("10 11 12 13 3 rev", "10 13 12 11");
        }

        [Test]
        public void TestDefWord()
        {
            Check("{ swap drop } :my-nip defword " +
                "1 2 3 my-nip", "1 3");
        }

        [Test]
        public void TestPick()
        {
            Check("10 11 12 13 3 pick", "10 11 12 13 11");
        }

        [Test]
        public void TestRoll()
        {
            Check("1 2 3 4 3 roll", "1 3 4 2");
            aip.stack.Clear();
            Check("1 2 3 4 -3 roll", "1 4 2 3");
        }
    }
}