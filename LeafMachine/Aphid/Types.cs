using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Linq;
using System;

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
        public virtual void Run(AphidInterpreter aip)
        {
            throw new System.Exception("not implemented");
        }

        // NOTE: ToString() is (currently) really only used for tests, to have a readable
        // string representation. No assumptions should be made, e.g. re-parsing the
        // value returned by ToString() does not necessarily produce the same object.
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

        // TODO: A method that indicates that we *run* this. NOT the same as Execute(), which shouldn't
        // even be possible for this type, because it happens directly after parsing, and there is no
        // way the parser can produce an AphidBuiltinWord directly.
        public override void Run(AphidInterpreter aip)
        {
            builtinWord(aip);
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

        public override void Execute(AphidInterpreter aip)
        {
            if (value.StartsWith(':')) {
                // it's a symbol literal; push the symbol onto the stack
                AphidSymbol sym = new AphidSymbol(value.Substring(1));
                // TODO: assert new symbol is not empty!
                aip.stack.Push(sym);
            } else {
                // look it up, expect to find a (built-in?) word, and run it
                AphidType x = aip.Lookup(this.value);
                x.Run(aip);
            }
        }
    }

    public class AphidList : AphidType 
    {
        List<AphidType> values;

        public AphidList()
        {
            values = new List<AphidType> { };
        }
        public AphidList(Stack s)
        {
            values = s.AsList();
        }
        public AphidList(List<AphidType> lst)
        {
            values = lst;
        }
        public override string ToString()
        {
            if (values.Count == 0) return "[ ]";
            string s = String.Join(' ', values.Select(x => x.ToString()));
            return $"[ {s} ]";
        }
    }


}