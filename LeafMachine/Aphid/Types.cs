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

        /* NOTE:
         * Execute() is what happens when a get an object of this type back after being parsed.
         * In many cases, this means it will be pushed onto the stack, but for some types,
         * other things will happen (e.g. words get executed, etc.)
         * (It really needs a better name...)
         */

        public virtual void Execute(AphidInterpreter aip)
            // Execute() is what happens when we encounter this type *in code*. Often, that means
            // it's pushed on the stack.
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
        public int AsInteger() { return value; }
    }

    public abstract class AphidWord : AphidType
    {
        protected string name;
        // a word must always have a name
    }

    public class AphidBuiltinWord : AphidWord
    {
        DelAphidBuiltinWord builtinWord;
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
        public string GetValue() { return value; }  // may or may not remain the same as ToString()

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
        public List<AphidType> AsList()
        {
            return values;
        }
    }

    public class AphidBlock : AphidType
    {
        List<AphidType> words;

        public AphidBlock()
        {
            words = new List<AphidType> { };
        }
        public AphidBlock(List<AphidType> some_words)
        {
            words = some_words;
        }

        public override string ToString()
        {
            if (words.Count == 0) return "{ }";
            string s = String.Join(' ', words.Select(x => x.ToString()));
            return $"{{ {s} }}";

        }
        public override void Execute(AphidInterpreter aip)
        {
            aip.stack.Push(this);
        }

        public override void Run(AphidInterpreter aip)
        {
            this.words.ForEach(x => x.Execute(aip));
        }
    }

    public class AphidString : AphidType
    {
        private string value = "";
        public AphidString() { value = ""; }
        public AphidString(string s) { value = s; }
        public override string ToString()
        {
            return $"\"{value}\"";
        }
        public override void Execute(AphidInterpreter aip)
        {
            aip.stack.Push(this);
        }
        public string AsString() { return value; }

    }

    public class AphidNull : AphidType
    {
        public AphidNull() { }
        public override string ToString()
        {
            return "null";
        }
        public override void Execute(AphidInterpreter aip)
        {
            aip.stack.Push(this);
        }
    }

}