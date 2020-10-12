using System.Collections.Generic;  // avoid collision with NUnit.Framework.List
using NUnit.Framework;
using LeafMachine.Aphid;

namespace LeafMachineTests
{
    public class AphidInterpreterTests
    {
        [SetUp]
        public void Setup()
        {
            AphidInterpreter aip = new AphidInterpreter();
        }

        [Test]
        public void Test1()
        {
        }
    }
}