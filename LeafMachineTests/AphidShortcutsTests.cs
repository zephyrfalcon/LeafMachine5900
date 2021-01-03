using System.Collections.Generic;
using NUnit.Framework;
using LeafMachine.Aphid;

namespace AphidShortcutsTests
{
    public class Tests
    {
        ShortcutExpander exp;

        [SetUp]
        public void Setup()
        {
            exp = new ShortcutExpander();
        }

        [Test]
        public void TestExpandVariables()
        {
            Assert.AreEqual(null, exp.Expand("bogus"));
            Assert.AreEqual(new List<string> { ":foo", "getvar" }, exp.Expand("$foo"));
            Assert.AreEqual(new List<string> { ":bar", "setvar" }, exp.Expand("$!bar"));
        }
        
        [Test]
        public void TestExpandTokens()
        {
            List<string> tokens = new List<string> { "42", "$!foo", "$foo", "dup" };
            List<string> expanded = new List<string> { "42", ":foo", "setvar", ":foo", "getvar", "dup" };
            Assert.AreEqual(exp.ExpandTokens(tokens), expanded);
        }
    }
}