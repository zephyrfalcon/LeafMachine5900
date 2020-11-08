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
            AphidType a = aip.stack.Pop();
            AphidType b = aip.stack.Pop();
            aip.stack.Push(a);
            aip.stack.Push(b);
        }

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
            AphidType x = aip.stack.Pop();
            if (x is AphidBlock)
                x.Run(aip);
            else throw new System.Exception($"exec: Cannot execute {x.ToString()}");
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

        public void Plus(AphidInterpreter aip)
        {
            AphidType x = aip.stack.Pop();
            AphidType y = aip.stack.Pop();
            int ix, iy;

            if (x is AphidInteger) {
                ix = (x as AphidInteger).AsInteger();
            }
            else throw new Exception($"+: integer expected, got {x.ToString()} instead");

            if (y is AphidInteger) {
                iy = (y as AphidInteger).AsInteger();
            }
            else throw new Exception($"+: integer expected, got {y.ToString()} instead");

            AphidInteger z = new AphidInteger(ix + iy);
            aip.stack.Push(z);
        }

        public void ForEach(AphidInterpreter aip)
        {
            // ( list block -- )
            // For each element in <list>, push it on the stack, then execute <block>.
            AphidType block = aip.stack.Pop();
            AphidType list = aip.stack.Pop();
            AphidBlock blk;
            AphidList lst;

            if (block is AphidBlock) {
                blk = (block as AphidBlock);
            }
            // TODO: allow a symbol to look up and use a built-in word?
            else throw new Exception($"for-each: block expected, got {block.ToString()} instead");

            if (list is AphidList) {
                lst = (list as AphidList);
            }
            else throw new Exception($"for-each: list expected, got {list.ToString()} instead");

            lst.AsList().ForEach(x => {
                aip.stack.Push(x);
                blk.Run(aip);
            });
        }

        public void Pack(AphidInterpreter aip)
        {
            // ( ...N items... N -- [...N items...] )
            AphidType an = aip.stack.Pop();
            if (an is AphidInteger) {
                int n = (an as AphidInteger).AsInteger();
                List<AphidType> list = new List<AphidType> { };
                for (int i=0; i < n; i++) {
                    AphidType x = aip.stack.Pop();
                    list.Add(x);
                }
                list.Reverse();
                aip.stack.Push(new AphidList(list));
            }
            else throw new Exception($"pack: integer expected, got {an.ToString()} instead");
        }

        public void Unpack(AphidInterpreter aip)
        {
            // ( list -- ...elements of list... )
        }

        /* built-in words */
        public Dictionary<string, DelAphidBuiltinWord> GetBuiltinWords()
        {
            Dictionary<string, DelAphidBuiltinWord> bw = new Dictionary<string, DelAphidBuiltinWord> {
                { "dup", Dup },
                { "drop", Drop },
                { "swap", Swap },
                { "[", LeftBracket },
                { "]", RightBracket },
                { "exec", Exec },
                { "str>chars", StrToChars },
                { "null", Null },
                { "setvar", SetVar },
                { "getvar", GetVar },
                { "+", Plus },
                { "for-each", ForEach },
                { "pack", Pack },
            };
            return bw;
        }
    }
}