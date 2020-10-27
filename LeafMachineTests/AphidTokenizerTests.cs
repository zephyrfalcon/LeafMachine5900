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
            string s1 = "  1 2 3   dup drop  döf 'x' ";
            List<string> values = new List<string> { "1", "2", "3", "dup", "drop", "döf", "'x'" };
            List<string> tokens = Tokenizer.Tokenize(s1);
            Assert.AreEqual(tokens, values);

            // try some Unicode stuff here
            Assert.AreEqual(tokens[5].Length, 3);
            char c = tokens[5][1];
            Assert.AreEqual(c, 'ö');
            // apparently it does not work for "😂", which is considered to have length 2.
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
        public void TestComments1()
        {
            List<string> tokens = Tokenizer.Tokenize("1 2 ; nonsense here\n  3 4");
            List<string> expected = new List<string> { "1", "2", "3", "4" };
            Assert.AreEqual(expected, tokens);
        }

        [Test]
        public void TestComments2()
        {
            List<string> tokens = Tokenizer.Tokenize("1 2 ; more nonsense here");
            List<string> expected = new List<string> { "1", "2" };
            Assert.AreEqual(expected, tokens);
        }

        [Test]
        public void TestStringsWithWhitespace()
        {
            List<string> tokens = Tokenizer.Tokenize("1 \"i like cookies\" 2");
            List<string> expected = new List<string> { "1", "\"i like cookies\"", "2" };
            Assert.AreEqual(expected, tokens);
        }
    }
}