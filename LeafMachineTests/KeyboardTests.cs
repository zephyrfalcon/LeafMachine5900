using System.Collections.Generic;
using NUnit.Framework;
using LeafMachine;

namespace LeafMachineTests
{
    public class KeyboardTests
    {
        KeyCodes keycodes;

        [SetUp]
        public void Setup()
        {
            keycodes = new KeyCodes();
        }

        [Test]
        public void TestKeyToName()
        {
            Assert.AreEqual(keycodes.KeyToName(Microsoft.Xna.Framework.Input.Keys.A), "a");
            Assert.AreEqual(keycodes.KeyToName(Microsoft.Xna.Framework.Input.Keys.Enter), "enter");
        }

        [Test]
        public void TestNameToKey()
        {
            Assert.AreEqual(keycodes.NameToKey("b"), Microsoft.Xna.Framework.Input.Keys.B);

            KeyNotFoundException ex = Assert.Throws<KeyNotFoundException>(
                delegate { keycodes.NameToKey("does-not-exist-ever"); });
        }
    }
}