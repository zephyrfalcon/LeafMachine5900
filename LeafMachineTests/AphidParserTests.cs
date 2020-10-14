using System.Collections.Generic;  // avoid collision with NUnit.Framework.List
using NUnit.Framework;
using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;
using System.Linq;

namespace LeafMachineTests
{
    public class AphidParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestParser()
        {
            List<AphidType> values = Parser.TokenizeAndParse("1 2 3 dup");
            Assert.AreEqual(values.Count, 4);
            Assert.AreEqual(values[0].ToString(), "1");
            Assert.AreEqual(values[1].ToString(), "2");
            Assert.AreEqual(values[2].ToString(), "3");
            Assert.AreEqual(values[3].ToString(), "dup");

            // we don't have map() but this appears to be the closest thing...
            string[] actual = values.Select(x => x.ToString()).ToArray();
            string[] expected = { "1", "2", "3", "dup" };
            Assert.AreEqual(actual, expected);

            Assert.IsTrue(values[1] is AphidInteger);
            Assert.IsTrue(values[3] is AphidSymbol);
        }
    }
}