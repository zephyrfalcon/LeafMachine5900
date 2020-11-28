using System.Collections.Generic;  // avoid collision with NUnit.Framework.List
using NUnit.Framework;
using LeafMachine.Aphid.Types;

namespace LeafMachineTests
{
    public class AphidTypeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAphidList()
        {
            List<AphidType> stuff = new List<AphidType> { new AphidInteger(1), new AphidSymbol("foo") };
            AphidList l1 = new AphidList(stuff);
            Assert.AreEqual(l1.ToString(), "[ 1 foo ]");

            AphidList l2 = new AphidList();
            Assert.AreEqual(l2.ToString(), "[ ]");
        }

        [Test]
        public void TestAphidDictionary()
        {
            AphidDictionary d = new AphidDictionary();
            d.Add(new AphidInteger(1), new AphidString("one"));
            d.Add(new AphidInteger(2), new AphidString("two"));
            d.Add(new AphidInteger(3), new AphidString("three"));
            Assert.AreEqual(d.Keys().Count, 3);
            Assert.AreEqual(d.GetDict()[new AphidInteger(1)], new AphidString("one"));

            d.Add(new AphidInteger(1), new AphidString("one again"));
            Assert.AreEqual(d.Keys().Count, 3);

            // check that we can add strings and symbols of value "foo" (but different types)
            // and that they are treated as two different keys
            d.Add(new AphidString("foo"), new AphidString("bar"));
            d.Add(new AphidSymbol("foo"), new AphidSymbol("baz"));
            Assert.AreEqual(d.Keys().Count, 5);
            Assert.AreEqual(d.GetDict()[new AphidString("foo")], new AphidString("bar"));
            Assert.AreEqual(d.GetDict()[new AphidSymbol("foo")], new AphidSymbol("baz"));

            // try adding a key of a type that isn't allowed
            // compiler doesn't let you! good
            //AphidBlock b = new AphidBlock();
            //d.Add(b, new AphidString("block"));
        }
    }
}