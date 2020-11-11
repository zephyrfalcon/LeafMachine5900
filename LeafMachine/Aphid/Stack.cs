using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using LeafMachine.Aphid.Types;

namespace LeafMachine.Aphid
{
    public class Stack
    {
        private List<AphidType> stack;

        public Stack()
        {
            this.stack = new List<AphidType>();
        }

        public void Push(AphidType x)
        {
            stack.Add(x);
        }
        public AphidType Pop()
        {
            if (stack.Count > 0) {
                AphidType x = this.stack.ElementAt(stack.Count - 1);
                stack.RemoveAt(stack.Count - 1);
                return x;
            }
            else {
                throw new Exception("stack underflow");
            }
        }

        public void Clear()
        {
            this.stack.Clear();
        }

        public bool IsEmpty()
        {
            return this.stack.Count == 0;
        }

        public AphidType TOS()
        {
            if (stack.Count > 0)
            {
                return this.stack.ElementAt(stack.Count - 1);
            }
            throw new Exception("stack underflow");
        }

        public int Size()
        {
            return this.stack.Count;
        }

        public AphidType Nth(int n)
            // get the n-th element of the stack, starting at the top, from 1.
            // (so the top of the stack is 1, the one under that is 2, etc.)
        {
            if (n > stack.Count)
                throw new Exception("stack underflow");
            if (n <= 0)
                throw new Exception($"cannot access stack with negative index");
            return this.stack.ElementAt(stack.Count - n);
        }

        public void DeleteNth(int n)
        {
            if (n > stack.Count)
                throw new Exception("stack underflow");
            if (n <= 0)
                throw new Exception($"cannot access stack with negative index");
            this.stack.RemoveAt(stack.Count - n);
        }

        // TODO: insert before or after Nth

        // simple stack representation meant for testing
        public string SimpleRepr()
        {
            return String.Join(" ", this.stack.Select(x => x.ToString()));
        }

        public List<AphidType> AsList()
        {
            return stack;
        }

    }
}