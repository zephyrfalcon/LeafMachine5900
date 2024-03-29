﻿using System.Collections.Generic;  // avoid collision with NUnit.Framework.List
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
        public void TestDivide()
        {
            Check("10 3 /", "3");
            aip.stack.Clear();
            Check("11 3 /", "3");
            aip.stack.Clear();
            Check("-4 -2 /", "2");
        }

        [Test]
        public void Testrem()
        {
            Check("10 3 rem", "1");
            aip.stack.Clear();
            Check("11 3 rem", "2");
            aip.stack.Clear();
            Check("10 5 rem", "0");
            aip.stack.Clear();
            Check("-21 4 rem", "-1");
            aip.stack.Clear();
            Check("-10 3 rem", "-1");
            aip.stack.Clear();
            Check("10 -3 rem", "1");  // for some reason; let's not worry about it right now
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

        [Test]
        public void TestIntToStr()
        {
            Check("42 int>str", "\"42\"");
        }

        [Test]
        public void TestIf()
        {
            // `if` can be used to execute code but can also leave stuff on the stack, which is what
            // we're doing here, since I don't have comparison words at this point yet :)
            Check("true { 4 } { 6 } if", "4");
            aip.stack.Clear();
            Check("false { :a } { :b } if", "b");
        }

        [Test]
        public void TestIntEquals()
        {
            Check("3 4 int= 5 5 int=", "false true");
        }

        [Test]
        public void TestIntGreaterThan()
        {
            Check("3 4 int>  4 4 int>  5 4 int>", "false false true");
        }

        [Test]
        public void TestLength()
        {
            Check("[ 1 2 3 ] length", "3");
            aip.stack.Clear();
            Check("\"abc def\" length", "7");
        }

        [Test]
        public void TestAt()
        {
            Check("[ :a :b :c ] 0 at", "a");
            aip.stack.Clear();
            Check("\"abc\" 1 at", "\"b\"");
        }

        [Test]
        public void TestSymbolEquals()
        {
            Check(":foo :foo symbol=", "true");
            aip.stack.Clear();
            Check(":foo :bar symbol=", "false");
        }

        [Test]
        public void TestAnd()
        {
            Check("{ 0 1 int= } { 2 2 int= } and", "false");
            aip.stack.Clear();
            Check("{ 1 1 int= } { 2 3 int= } and", "false");
            aip.stack.Clear();
            Check("{ 1 1 int= } { 2 2 int= } and", "true");
        }

        [Test]
        public void TestOr()
        {
            Check("{ 0 1 int= } { 2 2 int= } or", "true");
            aip.stack.Clear();
            Check("0 :block2-called setvar", "");
            Check("{ 1 1 int= } { 42 :block2-called setvar 2 2 int= } or", "true");
            aip.stack.Clear();
            Check(":block2-called getvar", "0");
        }

        [Test]
        public void TestNot()
        {
            Check("true not", "false");
        }

        [Test]
        public void TestDict()
        {
            Check("[ :a 1 :b 2 :c 3 ] dict", "[ a 1 b 2 c 3 ]!");
            aip.stack.Clear();
            Check("[ :d 4 :e 5 ]!", "[ d 4 e 5 ]!");  // from prelude
        }

        [Test]
        public void TestReturnStack()
        {
            Check("1 2 3 pushr", "1 2");  // do not clear the stack afterwards
            Check("swap 4 popr", "2 1 4 3");
        }

        [Test]
        public void TestFor()
        {
            Check("0 :counter setvar", "");
            Check("1 10 { :counter getvar + :counter setvar } for", "");
            Check(":counter getvar", "55");
        }

        [Test]
        public void TestArithmetic()
        {
            Check("10 3 -", "7");
            aip.stack.Clear();
            Check("4 10 -", "-6");
            aip.stack.Clear();
            Check("3 4 *", "12");
            aip.stack.Clear();
            Check("-4 -5 *", "20");
        }

        [Test]
        public void TestMap()
        {
            Check("[ 1 2 3 ] { 1 + } map", "[ 2 3 4 ]");
        }

        [Test]
        public void TestListSet()
        {
            Check("[ 1 2 3 ] :stuff setvar", "");
            Check("9 :stuff getvar 0 list-set", "[ 9 2 3 ]");
            aip.stack.Clear();
            Check(":stuff getvar", "[ 9 2 3 ]");
        }

        [Test]
        public void TestListReverse()
        {
            Check("[ 1 2 3 ] :stuff setvar", "");
            Check(":stuff getvar list-reverse", "[ 3 2 1 ]"); // list-reverse does push the modified list
            Check("drop :stuff getvar", "[ 3 2 1 ]");  // check that list was modified in-place
        }

        [Test]
        public void TestStringReverse()
        {
            Check("\"abc\" string-reverse", "\"cba\"");
            aip.stack.Clear();
            Check("\"Les Mise\u0301rables\" string-reverse", "\"selbare\u0301siM seL\"");
            // see: https://stackoverflow.com/a/15111719/27426
        }

        [Test]
        public void TestListSlice()
        {
            Check("[ 1 2 3 4 5 6 7 8 ] 0 2 list-slice", "[ 1 2 ]");
            aip.stack.Clear();
            Check("[ 1 2 3 4 5 6 7 ] 1 4 list-slice", "[ 2 3 4 ]");
        }

        [Test]
        public void TestListSetSlice()
        {
            Check("[ 1 2 3 4 5 6 7 ] :foo setvar", "");
            Check(":foo getvar 0 [ 0 0 0 ] list-set-slice", "[ 0 0 0 4 5 6 7 ]");
            aip.stack.Clear();
            Check(":foo getvar", "[ 0 0 0 4 5 6 7 ]");  // list was changed in-place
        }

        [Test]
        public void TestListReplace()
        {
            Check("[ 1 2 3 1 2 3 ] 1 4 list-replace", "[ 4 2 3 4 2 3 ]");
            aip.stack.Clear();
            // check that we modify the list in-place
            Check("[ 4 5 6 ] $!stuff", "");
            Check("$stuff 4 8 list-replace", "[ 8 5 6 ]");
            aip.stack.Clear();
            Check("$stuff", "[ 8 5 6 ]");
        }

        [Test]
        public void TestCopy()
        {
            Check("42 copy", "42");
            aip.stack.Clear();

            Check("[ 1 2 3 ] :foo setvar", "");               // foo = [1 2 3]
            Check(":foo getvar copy", "[ 1 2 3 ]");           // shallow copy is on the stack
            Check("9 swap 0 list-set", "[ 9 2 3 ]");          // set element 0 to 9
            Check("drop :foo getvar", "[ 1 2 3 ]");           // get contents of foo, they should still be [1 2 3]
        }

        [Test]
        public void TestForBy()
        {
            Check("[ 1 2 3 4 5 6 7 8 ] 2 { unpack + } for-by", "3 7 11 15");
        }

        [Test]
        public void TestOddOrEven()
        {
            Check("1 odd? 2 odd? -3 odd? 0 odd?", "true false true false");
            aip.stack.Clear();
            Check("1 even? 2 even? 22 even? 1001 even? -1 even? -4 even?", "false true true false false true");
        }

        [Test]
        public void TestFilter()
        {
            Check("[ 1 2 3 4 ] { odd? } filter", "[ 1 3 ]");
        }

        [Test]
        public void TestReduce()
        {
            Check("[ 1 2 3 4 ] { + } 0 reduce", "10");
            aip.stack.Clear();
            Check("[ 3 5 7 2 ] sum", "17");
        }

    }
}