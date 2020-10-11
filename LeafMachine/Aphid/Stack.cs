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
            if (stack.Count > 0)
            {
                AphidType x = this.stack.ElementAt(stack.Count - 1);
                stack.RemoveAt(stack.Count - 1);
                return x;
            }
            else
            {
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
        {
            throw new Exception("to be implemented");
        }

    }
}