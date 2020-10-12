using System.Security.Cryptography.X509Certificates;

namespace LeafMachine.Aphid.Types
{
    public class AphidType
    {
        // should this be abstract?

        // indicates what happens when we execute the object after parsing.
        // e.g. numbers => pushes onto the stack
        // symbols => looked up ...?
        // NEEDS MORE THOUGHT
        public virtual void Execute(AphidInterpreter aip)
        {
            throw new System.Exception("not implemented");
        }
    }

    public class AphidInteger : AphidType
    {
        private int value = 0;
        public AphidInteger() { }
        public AphidInteger(int x) { value = x; }
        public override string ToString() { return this.value.ToString(); }
        public override void Execute(AphidInterpreter aip)
        {
            aip.stack.Push(this);
        }
    }

    public class AphidBuiltinWord : AphidType
    {
        string name;
        DelAphidBuiltinWord builtinWord;
        // a built-in word must always have a name
        public AphidBuiltinWord(string aname, DelAphidBuiltinWord abuiltinWord)
        {
            name = aname;
            builtinWord = abuiltinWord;
        }

        public override string ToString()
        {
            return $"<{name}>";
        }
    }

    public class AphidSymbol : AphidType
    {
        string value = null;
        public AphidSymbol(string s)
        {
            value = s;
        }

        public override string ToString() { return value; }
    }

}