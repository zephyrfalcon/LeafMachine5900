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
            List<AphidType> values = Parser.TokenizeAndParse("1 2 3 dup \"hello\" 'y' 'ß' ");
            Assert.AreEqual(values.Count, 7);
            Assert.AreEqual(values[0].ToString(), "1");
            Assert.AreEqual(values[1].ToString(), "2");
            Assert.AreEqual(values[2].ToString(), "3");
            Assert.AreEqual(values[3].ToString(), "dup");
            Assert.AreEqual(values[4].ToString(), "\"hello\"");
            Assert.AreEqual(values[5].ToString(), "'y'");
            Assert.AreEqual(values[6].ToString(), "'ß'");

            // we don't have map() but this appears to be the closest thing...
            string[] actual = values.Select(x => x.ToString()).ToArray();
            string[] expected = { "1", "2", "3", "dup", "\"hello\"", "'y'", "'ß'" };
            Assert.AreEqual(actual, expected);

            // are the types correct?
            Assert.IsTrue(values[1] is AphidInteger);
            Assert.IsTrue(values[3] is AphidSymbol);
            Assert.IsTrue(values[4] is AphidString);
            Assert.IsTrue(values[5] is AphidChar);
            Assert.IsTrue(values[6] is AphidChar);
        }

        [Test]
        public void TestBlocks()
        {
            List<AphidType> values = Parser.TokenizeAndParse("{ 3 4 }");
            Assert.IsTrue(values[0] is AphidBlock);
        }
    }
}