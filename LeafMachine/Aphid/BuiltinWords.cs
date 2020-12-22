using System;
using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LeafMachine.Aphid
{
    public delegate void DelAphidBuiltinWord(AphidInterpreter aip);

    public class BuiltinWords
    {
        static Random rnd = new Random();

        #region stack_manipulation

        public void Dup(AphidInterpreter aip)
        {
            AphidType x = aip.stack.TOS();
            // DUP pushes the exact same object, so this has consequences for mutable objects
            aip.stack.Push(x);
        }

        public void Drop(AphidInterpreter aip)
        {
            aip.stack.Pop();
        }

        public void Swap(AphidInterpreter aip)
        {
            // ( a b -- b a )
            AphidType b = aip.stack.Pop();
            AphidType a = aip.stack.Pop();
            aip.stack.Push(b);
            aip.stack.Push(a);
        }

        public void Over(AphidInterpreter aip)
        {
            // ( n1 n2 -- n1 n2 n1 )
            AphidType n2 = aip.stack.Pop();
            AphidType n1 = aip.stack.Pop();
            aip.stack.Push(n1);
            aip.stack.Push(n2);
            aip.stack.Push(n1);
            // TODO: can be written more efficiently by not doing the pop-and-push-again dance
            // but by inspecting the TOS-1 and pushing that...
        }

        public void Rol(AphidInterpreter aip)
        {
            // ( n1 n2 n3 -- n2 n3 n1 )
            AphidType n3 = aip.stack.Pop();
            AphidType n2 = aip.stack.Pop();
            AphidType n1 = aip.stack.Pop();
            aip.stack.Push(n2);
            aip.stack.Push(n3);
            aip.stack.Push(n1);
        }

        public void Ror(AphidInterpreter aip)
        {
            // ( n1 n2 n3 -- n3 n1 n2 )
            AphidType n3 = aip.stack.Pop();
            AphidType n2 = aip.stack.Pop();
            AphidType n1 = aip.stack.Pop();
            aip.stack.Push(n3);
            aip.stack.Push(n1);
            aip.stack.Push(n2);
        }

        public void Nip(AphidInterpreter aip)
        {
            // ( a b -- b )
            aip.stack.DeleteNth(2);
        }

        public void Tuck(AphidInterpreter aip)
        {
            // ( a b -- b a b )
            AphidType b = aip.stack.Pop();
            AphidType a = aip.stack.Pop();
            aip.stack.Push(b);
            aip.stack.Push(a);
            aip.stack.Push(b);
        }

        #endregion

        public void LeftBracket(AphidInterpreter aip)
        {
            aip.PushStack();
        }

        public void RightBracket(AphidInterpreter aip)
        {
            Stack topstack = aip.PopStack();
            aip.stack.Push(new AphidList(topstack));
        }

        public void Exec(AphidInterpreter aip)
        {
            // ( block -- ? )
            // Execute the given block.
            AphidBlock blk = Expect.ExpectAphidBlock("exec", aip.stack.Pop());
            blk.Run(aip);
        }

        public void StrToChars(AphidInterpreter aip)
        {
            AphidType x = aip.stack.Pop();
            if (x is AphidString) {
                string s1 = x.ToString();
                string s2 = s1.Substring(1, s1.Length - 2);
                Char[] chars = s2.ToCharArray();  // maybe fix later to handle Unicode better?
                List<AphidType> achars = new List<AphidType> { };
                foreach (Char c in chars) {
                    achars.Add(new AphidString(c.ToString()));
                }
                aip.stack.Push(new AphidList(achars));
            }
            else throw new System.Exception($"str>chars: Not a string: {x.ToString()}");
        }

        public void Null(AphidInterpreter aip)
        {
            aip.stack.Push(new AphidNull());
        }

        public void SetVar(AphidInterpreter aip)
        {
            // ( value :name -- )
            AphidType name = aip.stack.Pop();
            AphidType value = aip.stack.Pop();

            if (name is AphidSymbol) {
                string s = (name as AphidSymbol).GetValue();
                aip.SetVar(s, value);
            }
            else throw new Exception($"setvar: symbol expected, got {name.ToString()} instead");
        }

        public void GetVar(AphidInterpreter aip)
        {
            // ( :name -- value )
            AphidType name = aip.stack.Pop();

            if (name is AphidSymbol) {
                string s = (name as AphidSymbol).GetValue();
                AphidType value = aip.GetVar(s);  // we assume the variable exists
                aip.stack.Push(value);
            }
            else throw new Exception($"getvar: symbol expected, got {name.ToString()} instead");
        }

        #region arithmetic

        public void Plus(AphidInterpreter aip)
        {
            int x = Expect.ExpectInteger("+", aip.stack.Pop());
            int y = Expect.ExpectInteger("+", aip.stack.Pop());
            AphidInteger z = new AphidInteger(x + y);
            aip.stack.Push(z);
        }

        public void Minus(AphidInterpreter aip)
        {
            // ( a b -- a-b )
            int b = Expect.ExpectInteger("-", aip.stack.Pop());
            int a = Expect.ExpectInteger("-", aip.stack.Pop());
            aip.stack.Push(new AphidInteger(a - b));
        }

        public void Multiply(AphidInterpreter aip)
        {
            // ( a b -- a*b )
            int b = Expect.ExpectInteger("*", aip.stack.Pop());
            int a = Expect.ExpectInteger("*", aip.stack.Pop());
            aip.stack.Push(new AphidInteger(a * b));
        }

        public void Divide(AphidInterpreter aip)
        {
            // ( a b -- a/b )
            // Divides a/b, using integer division.
            int b = Expect.ExpectInteger("/", aip.stack.Pop());
            int a = Expect.ExpectInteger("/", aip.stack.Pop());
            int c = a / b;
            aip.stack.Push(new AphidInteger(c));
        }

        public void Rem(AphidInterpreter aip)
        {
            // ( a b -- a%b )
            // Divides a/b, using integer division, putting the remainder on the stack.
            // (Like the % in many other languages, like C, Javascript and Python.)
            int b = Expect.ExpectInteger("rem", aip.stack.Pop());
            int a = Expect.ExpectInteger("rem", aip.stack.Pop());
            int c = a % b;
            aip.stack.Push(new AphidInteger(c));
        }

        #endregion

        public void ForEach(AphidInterpreter aip)
        {
            // ( list block -- )
            // For each element in <list>, push it on the stack, then execute <block>.
            AphidType block = aip.stack.Pop();
            AphidBlock blk;
            AphidList lst;

            if (block is AphidBlock) {
                blk = (block as AphidBlock);
            }
            // TODO: allow a symbol to look up and use a built-in word?
            else throw new Exception($"for-each: block expected, got {block.ToString()} instead");

            lst = Expect.ExpectAphidList("for-each", aip.stack.Pop());

            lst.AsList().ForEach(x => {
                aip.stack.Push(x);
                blk.Run(aip);
            });
        }

        public void For(AphidInterpreter aip)
        {
            // ( begin end block -- ? )
            // Loop from begin to end, inclusively, pushing the counter on the stack and executing
            // block each time.
            AphidBlock blk = Expect.ExpectAphidBlock("for", aip.stack.Pop());
            int end = Expect.ExpectInteger("for", aip.stack.Pop());
            int begin = Expect.ExpectInteger("for", aip.stack.Pop());
            // for now, ignore situations where begin > end
            if (end >= begin) {
                for (int i = begin; i <= end; i++) {
                    aip.stack.Push(new AphidInteger(i));
                    blk.Run(aip);
                }
            }
        }

        public void Map(AphidInterpreter aip)
        {
            // ( list block -- list' )
            // For each element in list, push it onto the stack, then call the given block, which is
            // expected to leave an item as well. Put this item into the new list.
            AphidBlock blk = Expect.ExpectAphidBlock("map", aip.stack.Pop());
            AphidList lst = Expect.ExpectAphidList("map", aip.stack.Pop());
            List<AphidType> results = new List<AphidType>();
            lst.AsList().ForEach(x => {
                aip.stack.Push(x);
                blk.Run(aip);
                results.Add(aip.stack.Pop());
            });
            aip.stack.Push(new AphidList(results));
        }

        public void Pack(AphidInterpreter aip)
        {
            // ( ...N items... N -- [...N items...] )
            int n = Expect.ExpectInteger("pack", aip.stack.Pop());
            List<AphidType> list = new List<AphidType> { };
            for (int i = 0; i < n; i++) {
                AphidType x = aip.stack.Pop();
                list.Add(x);
            }
            list.Reverse();
            aip.stack.Push(new AphidList(list));
        }

        public void Unpack(AphidInterpreter aip)
        {
            // ( list -- ...elements of list... )
            AphidList list = Expect.ExpectAphidList("unpack", aip.stack.Pop());
            list.AsList().ForEach(x => aip.stack.Push(x));
        }

        public void ThreeRev(AphidInterpreter aip)
        {
            // ( a b c -- c b a )
            AphidType a = aip.stack.Pop();
            AphidType b = aip.stack.Pop();
            AphidType c = aip.stack.Pop();
            aip.stack.Push(a);
            aip.stack.Push(b);
            aip.stack.Push(c);
        }

        public void FourRev(AphidInterpreter aip)
        {
            // ( a b c d -- d c b a )
            AphidType a = aip.stack.Pop();
            AphidType b = aip.stack.Pop();
            AphidType c = aip.stack.Pop();
            AphidType d = aip.stack.Pop();
            aip.stack.Push(a);
            aip.stack.Push(b);
            aip.stack.Push(c);
            aip.stack.Push(d);
        }

        public void Rev(AphidInterpreter aip)
        {
            // ( ...N items... N -- ...N items in reverse order... )
            int n = Expect.ExpectInteger("rev", aip.stack.Pop());
            List<AphidType> items = new List<AphidType>();
            for (int i = 0; i < n; i++) {
                items.Add(aip.stack.Pop());
            }
            items.ForEach(x => aip.stack.Push(x));
        }

        public void Roll(AphidInterpreter aip)
        {
            // ( ...N items... N -- ...N items rotated "to the left"... )
            // negative values rotate "to the right", and 0 is a no-op
            int n = Expect.ExpectInteger("roll", aip.stack.Pop());
            if (n == 0) return; // no-op
            List<AphidType> items = new List<AphidType>();
            if (n < 0) {
                n = -n;
                for (int i = 0; i < n; i++)
                    items.Add(aip.stack.Pop());
                AphidType x = items[0];
                items.RemoveAt(0);
                items.Add(x);
            } else if (n > 0) {
                for (int i = 0; i < n; i++)
                    items.Add(aip.stack.Pop());
                AphidType x = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                items.Insert(0, x);
            }
            items.Reverse();
            items.ForEach(x => aip.stack.Push(x));
        }

        public void Pick(AphidInterpreter aip)
        {
            // ( ...items... N -- <Nth item> )
            // where we start counting at 1, so the TOS is 1, the item under that is 2, etc
            int n = Expect.ExpectInteger("pick", aip.stack.Pop());
            if (aip.stack.Size() < n)
                throw new Exception($"pick: stack underflow, at least {n} elements expected");
            AphidType x = aip.stack.Nth(n);
            aip.stack.Push(x);
        }

        public void DefWord(AphidInterpreter aip)
        {
            // ( block :name -- )
            string name = Expect.ExpectSymbol("defword", aip.stack.Pop());
            AphidBlock block = Expect.ExpectAphidBlock("defword", aip.stack.Pop());
            AphidUserDefinedWord word = new AphidUserDefinedWord(name, block);
            aip.LoadWord(name, word);
        }

        public void IntToStr(AphidInterpreter aip)
        {
            // ( integer -- string )
            int i = Expect.ExpectInteger("int>str", aip.stack.Pop());
            aip.stack.Push(new AphidString(i.ToString()));
        }

        public void True(AphidInterpreter aip)
        {
            aip.stack.Push(new AphidBool(true));
        }

        public void False(AphidInterpreter aip)
        {
            aip.stack.Push(new AphidBool(false));
        }

        public void If(AphidInterpreter aip)
        {
            // ( bool true-block false-block -- ? )
            AphidBlock falseBlock = Expect.ExpectAphidBlock("if", aip.stack.Pop());
            AphidBlock trueBlock = Expect.ExpectAphidBlock("if", aip.stack.Pop());
            bool condition = Expect.ExpectBool("if", aip.stack.Pop());
            if (condition)
                trueBlock.Run(aip);
            else falseBlock.Run(aip);
        }

        public void IntEquals(AphidInterpreter aip)
        {
            // ( a b == bool )
            int b = Expect.ExpectInteger("int=", aip.stack.Pop());
            int a = Expect.ExpectInteger("int=", aip.stack.Pop());
            aip.stack.Push(new AphidBool(a == b));
        }

        public void IntGreaterThan(AphidInterpreter aip)
        {
            // ( a b == bool )
            int b = Expect.ExpectInteger("int>", aip.stack.Pop());
            int a = Expect.ExpectInteger("int>", aip.stack.Pop());
            aip.stack.Push(new AphidBool(a > b));
        }

        public void IntLessThan(AphidInterpreter aip)
        {
            // ( a b == bool )
            int b = Expect.ExpectInteger("int<", aip.stack.Pop());
            int a = Expect.ExpectInteger("int<", aip.stack.Pop());
            aip.stack.Push(new AphidBool(a < b));
        }


        public void Length(AphidInterpreter aip)
        {
            // ( list|string -- length )
            AphidType x = aip.stack.Pop();
            if (x is AphidList) {
                aip.stack.Push(new AphidInteger((x as AphidList).AsList().Count));
            }
            else if (x is AphidString) {
                aip.stack.Push(new AphidInteger((x as AphidString).AsString().Length));
            }
            else throw new Exception($"length: list or string expected, got {x.ToString()} instead");
        }

        // XXX This is a generic word; it currently supports lists and strings, and in the future
        // could support dicts and maybe other types. Question is, if we should have type-specific
        // words instead: list-at, string-at, dict-at, etc.
        public void At(AphidInterpreter aip)
        {
            // ( list|string index -- value )
            int idx = Expect.ExpectInteger("at", aip.stack.Pop());  // may change in future if we support dicts
            AphidType x = aip.stack.Pop();
            if (x is AphidList) {
                aip.stack.Push((x as AphidList).AsList()[idx]);
            }
            else if (x is AphidString) {
                string s = (x as AphidString).AsString();
                string c = s[idx].ToString();
                aip.stack.Push(new AphidString(c));
            }
            else throw new Exception($"length: list or string expected, got {x.ToString()} instead");
        }

        public void ListSet(AphidInterpreter aip)
        {
            // ( value list index -- )
            // Sets element `index` of `list` to `value`. Modifies the list in-place.
            int idx = Expect.ExpectInteger("list-set", aip.stack.Pop());
            AphidList list = Expect.ExpectAphidList("list-set", aip.stack.Pop());
            AphidType value = aip.stack.Pop();
            list.AsList()[idx] = value;  // will throw an error if idx is out of bounds
            // TODO: support negative indexes?
        }

        public void ListReverse(AphidInterpreter aip)
        {
            // ( list -- )
            // Reverses a list in-place.
            AphidList list = Expect.ExpectAphidList("list-reverse", aip.stack.Pop());
            list.AsList().Reverse();
        }

        public void ListSlice(AphidInterpreter aip)
        {
            // ( list start stop -- list[start:stop] )
            // where `start` is inclusive but `stop` is exclusive
            int stop = Expect.ExpectInteger("list-slice", aip.stack.Pop());
            int start = Expect.ExpectInteger("list-slice", aip.stack.Pop());
            AphidList list = Expect.ExpectAphidList("list-slice", aip.stack.Pop());
            // TODO: assert start >= stop
            List<AphidType> slice = list.AsList().GetRange(start, stop - start);
            aip.stack.Push(new AphidList(slice));
        }

        public void ListSetSlice(AphidInterpreter aip) {
            // ( list start slice -- list )
            // Replace the element of a list, starting at position `start`, with the element
            // of list `slice`. The number of elements replaced is the same as the length
            // of `slice`.
            // Changes the list in-place. (Should it?)
            // TODO: What happens if `slice` is longer than `list`?
            AphidList slice = Expect.ExpectAphidList("list-set-slice", aip.stack.Pop());
            int start = Expect.ExpectInteger("list-set-slice", aip.stack.Pop());
            AphidList list = Expect.ExpectAphidList("list-set-slice", aip.stack.Pop());
            list.AsList().RemoveRange(start, slice.AsList().Count);
            list.AsList().InsertRange(start, slice.AsList());
            aip.stack.Push(list);
        }

        public void StringReverse(AphidInterpreter aip)
        {
            // ( string -- reversed-string )
            string s = Expect.ExpectString("string-reverse", aip.stack.Pop());
            string t = StringReverser.ReverseGraphemeClusters(s);
            aip.stack.Push(new AphidString(t));
            // see: https://stackoverflow.com/a/15111719/27426
        }

        public void SymbolEquals(AphidInterpreter aip)
        {
            // ( symbol1 symbol2 -- bool )
            // True if the two symbols are equal, false otherwise
            string s2 = Expect.ExpectSymbol("symbol=", aip.stack.Pop());
            string s1 = Expect.ExpectSymbol("symbol=", aip.stack.Pop());
            aip.stack.Push(new AphidBool(s1 == s2));
        }

        public void Random(AphidInterpreter aip)
        {
            // ( n -- [random number >= 0 and < n] )
            int n = Expect.ExpectInteger("random", aip.stack.Pop());
            aip.stack.Push(new AphidInteger(rnd.Next(0, n)));
        }

        public void And(AphidInterpreter aip)
        {
            // ( block1 block2 -- bool )
            // the blocks are expected to leave a boolean on the stack.
            // shortcut behavior: block2 is not executed if block1 leaves `false` on the stack.
            AphidBlock block2 = Expect.ExpectAphidBlock("and", aip.stack.Pop());
            AphidBlock block1 = Expect.ExpectAphidBlock("and", aip.stack.Pop());
            block1.Run(aip);
            bool b1 = Expect.ExpectBool("and (code block 1)", aip.stack.Pop());
            if (b1) {
                block2.Run(aip);
                bool b2 = Expect.ExpectBool("and (code block 2)", aip.stack.Pop());
                aip.stack.Push(new AphidBool(b2));
            }
            else aip.stack.Push(new AphidBool(false));
        }

        public void Or(AphidInterpreter aip)
        {
            // ( block1 block2 -- bool )
            // The blocks are expected to leave a boolean on the stack.
            // Shortcut behavior: if block1 leaves `true` on the stack, block2 is not executed.
            AphidBlock block2 = Expect.ExpectAphidBlock("or", aip.stack.Pop());
            AphidBlock block1 = Expect.ExpectAphidBlock("or", aip.stack.Pop());
            block1.Run(aip);
            bool b1 = Expect.ExpectBool("or (code block 1)", aip.stack.Pop());
            if (b1) aip.stack.Push(new AphidBool(true));
            else {
                block2.Run(aip);
                bool b2 = Expect.ExpectBool("or (code block 2)", aip.stack.Pop());
                aip.stack.Push(new AphidBool(b2));
            }
        }

        public void Not(AphidInterpreter aip)
        {
            // ( bool -- !bool )
            bool b = Expect.ExpectBool("not", aip.stack.Pop());
            aip.stack.Push(new AphidBool(!b));
        }

        public void Dict(AphidInterpreter aip)
        {
            // ( list -- dict )
            // The list is expected to have an even number of elements, alternating between
            // key and value. E.g. [ :a 1 :b 2 ... ]
            AphidList alist = Expect.ExpectAphidList("dict", aip.stack.Pop());
            List<AphidType> list = alist.AsList();
            if (list.Count % 2 != 0)
                throw new Exception($"dict: source list must have an even number of elements (got {list.Count} instead)");
            AphidDictionary d = new AphidDictionary();
            for (int i = 0; i < list.Count; i += 2) {
                AphidType key = list[i];
                AphidType value = list[i + 1];
                if (key is IDictionaryKey) {
                    d.Add(key as IDictionaryKey, value);
                } else {
                    throw new Exception($"dict: not a valid dictionary key: {key.ToString()}");
                }
            }
            aip.stack.Push(d);
        }

        #region returnstack;

        public void PushR(AphidInterpreter aip)
        {
            // ( x -- )
            // RS: ( -- x )
            // Pop the TOS and push it onto the return stack.
            AphidType x = aip.stack.Pop();
            aip.ReturnStack.Push(x);
        }

        public void PopR(AphidInterpreter aip)
        {
            // ( -- x)
            // RS: ( x -- )
            // Pop the TOS from the return stack and push it onto the data stack.
            AphidType x = aip.ReturnStack.Pop();
            aip.stack.Push(x);
        }

        #endregion returnstack;

        /* built-in words */
        public Dictionary<string, DelAphidBuiltinWord> GetBuiltinWords()
        {
            Dictionary<string, DelAphidBuiltinWord> bw = new Dictionary<string, DelAphidBuiltinWord> {
                { "dup", Dup },
                { "drop", Drop },
                { "swap", Swap },
                { "over", Over },
                { "rol", Rol },
                { "ror", Ror },
                { "nip", Nip },
                { "tuck", Tuck },
                { "[", LeftBracket },
                { "]", RightBracket },
                { "exec", Exec },
                { "str>chars", StrToChars },
                { "null", Null },
                { "true", True },
                { "false", False },
                { "setvar", SetVar },
                { "getvar", GetVar },
                { "+", Plus },
                { "-", Minus },
                { "*", Multiply },
                { "/", Divide },
                { "rem", Rem },

                { "for-each", ForEach },
                { "for", For },
                { "map", Map },

                { "pack", Pack },
                { "unpack", Unpack },
                { "3rev", ThreeRev },  // maybe redundant
                { "4rev", FourRev },   // maybe redundant
                { "rev", Rev },
                { "defword", DefWord },
                { "pick", Pick },
                { "roll", Roll },
                { "int>str", IntToStr },  // should be replaced by general 'to-string' word
                { "if", If },
                { "int=", IntEquals },
                { "int>", IntGreaterThan },
                { "int<", IntLessThan },
                { "length", Length },
                { "at", At },
                { "symbol=", SymbolEquals },
                { "random", Random },
                { "and", And },
                { "or", Or },
                { "not", Not },
                { "dict", Dict },
                { "pushr", PushR },
                { "popr", PopR },
                { "list-set", ListSet },
                { "list-reverse", ListReverse },
                { "string-reverse", StringReverse },
                { "list-slice", ListSlice },
                { "list-set-slice", ListSetSlice },
            };
            return bw;
        }
    }
}