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
            string[] values = { "1", "2", "3", "dup", "drop" };
            string[] tokens = Tokenizer.Tokenize(s1);
            Assert.AreEqual(tokens, values);
        }

        [Test]
        public void TestNewlines()
        {
            string[] tokens = Tokenizer.Tokenize("1 2\n3 4");
            string[] expected = { "1", "2", "3", "4" };
            Assert.AreEqual(expected, tokens);
        }

        [Test]
        public void TestEmptyLines()
        {
            string[] tokens = Tokenizer.Tokenize("1 2\n\n3\r\t\t\n\r  5\n");
            string[] expected = { "1", "2", "3", "5" };
            Assert.AreEqual(expected, tokens);
        }

        [Test]
        public void TestComments()
        {
            string[] tokens = Tokenizer.Tokenize("1 2 ; nonsense here\n  3 4");
            string[] expected = { "1", "2", "3", "4" };
            Assert.AreEqual(expected, tokens);
        }
    }
}