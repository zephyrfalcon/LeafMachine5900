using NUnit.Framework;
using LeafMachine.CharSets;
using System.Collections.Generic;

namespace LeafMachineTests
{
    public class CharSetTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestC64CharSetIndexes()
        {
            C64CharSet cs = new C64CharSet();
            Dictionary<char, int> d = cs.CharToBitmapIndex();
            Assert.AreEqual(0, d['@']);
            Assert.AreEqual(1, d['A']);
            Assert.AreEqual(48, d['0']);
            Assert.AreEqual(129, d['a']);
        }
    }
}