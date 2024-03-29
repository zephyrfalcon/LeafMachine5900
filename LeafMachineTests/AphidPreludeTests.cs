﻿using System.Collections.Generic;  // avoid collision with NUnit.Framework.List
using NUnit.Framework;
using LeafMachine.Aphid;
using NuGet.Frameworks;
using LeafMachine.Aphid.Types;

namespace LeafMachineTests
{
    public class AphidPreludeTests
    {
        AphidInterpreter aip;

        [SetUp]
        public void Setup()
        {
            this.aip = new AphidInterpreter();
        }

        public void Check(string code, string stackRepr)
        {
            aip.Run(code);
            Assert.AreEqual(stackRepr, aip.stack.SimpleRepr());
        }

        [Test]
        public void TestWhen()
        {
            Check("true { 33 } when", "33");
            aip.stack.Clear();
            Check("false { 44 } when", "");
        }

        [Test]
        public void TestInc()
        {
            Check("3 :foo setvar  :foo inc  :foo getvar", "4");
        }

        [Test]
        public void TestDec()
        {
            Check("42 :bar setvar  :bar dec  :bar getvar", "41");
        }

    }
}