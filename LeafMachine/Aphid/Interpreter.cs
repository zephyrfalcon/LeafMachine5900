using System;
using System.Collections.Generic;
using System.Text;

using LeafMachine.Aphid.Types;
using LeafMachine.Aphid;
using System.Linq;
using System.ComponentModel;
using System.Threading;

namespace LeafMachine.Aphid
{
    public class AphidInterpreter
    {
        private List<Stack> stacks;
        private Dictionary<string, AphidWord> words;  // should this be: AphidWord?
        private Dictionary<string, AphidType> variables;

        public AphidInterpreter()
        {
            stacks = new List<Stack> { new Stack() };
            variables = new Dictionary<string, AphidType> { };
            LoadBuiltins();
        }

        // We normally only work with the topmost Stack, which for convenience can be referred to
        // as 'stack'. :) There will be a *few* functions that manipulate `stacks` directly, but
        // those should be rare enough to warrant the naming convention.
        public Stack stack
        {
            get { return stacks.ElementAt(stacks.Count - 1); }
        }

        public void PushStack()
        {
            stacks.Add(new Stack());
        }

        public Stack PopStack()
        {
            if (stacks.Count > 1) {
                Stack topstack = stacks.ElementAt(stacks.Count - 1);
                stacks.RemoveAt(stacks.Count - 1);
                return topstack;
            } else {
                throw new Exception("cannot collapse stack");
            }
        }

        // TODO: AddStack, which pushes a new Stack onto `stacks`

        protected void LoadBuiltins()
        {
            BuiltinWords bw = new BuiltinWords();
            words = new Dictionary<string, AphidWord> { };
            foreach (KeyValuePair<string, DelAphidBuiltinWord> entry in bw.GetBuiltinWords()) {
                LoadBuiltin(entry.Key, new AphidBuiltinWord(entry.Key, entry.Value));
            }
        }

        public void LoadBuiltin(string name, AphidWord word)
        {
            // TODO: make sure we don't have duplicates
            if (words.ContainsKey(name))
                throw new Exception($"Word already exists: {name}");
            words[name] = word;
        }

        public void Run(string s)
        {
            List<AphidType> values = Parser.TokenizeAndParse(s);
            values.ForEach(x => x.Execute(this));
        }

        public void RunFile(string filename)
        {
            string code = System.IO.File.ReadAllText(filename);
            Run(code);
        }

        public AphidWord Lookup(string s)
        {
            AphidWord result = null;
            bool found = this.words.TryGetValue(s, out result);
            if (found) return result; 
            else throw new Exception($"word not found: {s}");
        }

        public void Reset()
        {
            stacks.Clear();
        }

        public void SetVar(string name, AphidType x)
        {
            variables[name] = x;
        }

        public AphidType GetVar(string name)
        {
            return variables[name];
        }
    }
}