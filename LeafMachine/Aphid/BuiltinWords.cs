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
            int x = Expect.ExpectInteger("+", aip.stack.Pop());
            int y = Expect.ExpectInteger("+", aip.stack.Pop());
            AphidInteger z = new AphidInteger(x + y);
            aip.stack.Push(z);
        }

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
                { "unpack", Unpack },
                { "3rev", ThreeRev },
                { "4rev", FourRev },
                { "rev", Rev },
            };
            return bw;
        }
    }
}