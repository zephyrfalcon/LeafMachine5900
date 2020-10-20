using System.Collections.Generic;  // avoid collision with NUnit.Framework.List
using NUnit.Framework;
using LeafMachine.Aphid;

namespace LeafMachineTests
{
    public class AphidTokenizerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBasics()
        {
            string s1 = "  1 2 3   dup drop ";
            List<string> values = new List<string> { "1", "2", "3", "dup", "drop" };
            List<string> tokens = Tokenizer.Tokenize(s1);
            Assert.AreEqual(tokens, values);
        }

        [Test]
        public void TestNewlines()
        {
            List<string> tokens = Tokenizer.Tokenize("1 2\n3 4");
            List<string> expected = new List<string> { "1", "2", "3", "4" };
            Assert.AreEqual(expected, tokens);
        }

        [Test]
        public void TestEmptyLines()
        {
            List<string> tokens = Tokenizer.Tokenize("1 2\n\n3\r\t\t\n\r  5\n");
            List<string> expected = new List<string> { "1", "2", "3", "5" };
            Assert.AreEqual(expected, tokens);
        }

        [Test]
        public void TestComments()
        {
            List<string> tokens = Tokenizer.Tokenize("1 2 ; nonsense here\n  3 4");
            List<string> expected = new List<string> { "1", "2", "3", "4" };
            Assert.AreEqual(expected, tokens);
        }
    }
}