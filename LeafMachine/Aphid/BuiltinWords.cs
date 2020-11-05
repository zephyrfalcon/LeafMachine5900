using System;
using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;
using System.Collections.Generic;

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
            };
            return bw;
        }
    }
}