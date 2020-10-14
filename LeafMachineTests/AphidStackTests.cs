using NUnit.Framework;
using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;

namespace LeafMachineTests
{
    public class AphidStackTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBasics()
        {
            Stack s = new Stack();
            Assert.IsTrue(s.IsEmpty());
            s.Push(new AphidInteger(1));
            Assert.AreEqual(s.Size(), 1);
            s.Push(new AphidInteger(2));
            Assert.AreEqual(s.Size(), 2);

            AphidType x = s.Pop();
            Assert.AreEqual(s.Size(), 1);
            // Assert.AreEqual(x, new AphidInteger(2));   // does not work yet
            // add tests for equality later in the test suite for types! for now, just wing it :)
            Assert.AreEqual(x.ToString(), "2");
        }

        [Test]
        public void TestSimpleRepr()
        {
            Stack s = new Stack();
            s.Push(new AphidInteger(1));
            s.Push(new AphidInteger(2));
            s.Push(new AphidInteger(3));
            Assert.AreEqual(s.SimpleRepr(), "1 2 3");

            Stack s2 = new Stack();
            Assert.AreEqual(s2.SimpleRepr(), "");
        }
    }
}