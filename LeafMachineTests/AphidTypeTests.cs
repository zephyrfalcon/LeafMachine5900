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
    }
}