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
        public void TestTokenizer()
        {
            string s1 = "  1 2 3   dup drop ";
            string[] values = { "1", "2", "3", "dup", "drop" };
            string[] tokens = Tokenizer.Tokenize(s1);
            Assert.AreEqual(tokens, values);
        }

        [Test]
        public void TestTokenizerNewlines()
        {
            string[] tokens = Tokenizer.Tokenize("1 2\n3 4");
            string[] expected = { "1", "2", "3", "4" };
            Assert.AreEqual(expected, tokens);
        }
    }
}