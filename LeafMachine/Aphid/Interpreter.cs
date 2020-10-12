using System;
using System.Collections.Generic;
using System.Text;

using LeafMachine.Aphid.Types;
using LeafMachine.Aphid;
using System.Linq;

namespace LeafMachine.Aphid
{
    public class AphidInterpreter
    {
        private List<Stack> stacks;
        private Dictionary<string, AphidType> words;

        public AphidInterpreter()
        {
            stacks = new List<Stack> { new Stack() };
            LoadBuiltins();
        }

        // We normally only work with the topmost Stack, which for convenience can be referred to
        // as 'stack'. :) There will be a *few* functions that manipulate `stacks` directly, but
        // those should be rare enough to warrant the naming convention.
        public Stack stack
        {
            get { return stacks.ElementAt(stacks.Count - 1); }
        }

        // TODO: AddStack, which pushes a new Stack onto `stacks`

        protected void LoadBuiltins()
        {
            BuiltinWords bw = new BuiltinWords();
            words = new Dictionary<string, AphidType> { };
            foreach (KeyValuePair<string, DelAphidBuiltinWord> entry in bw.GetBuiltinWords())
            {
                words[entry.Key] = new AphidBuiltinWord(entry.Key, entry.Value);
            }
            // TODO: make sure we don't have duplicates
        }

        public void Run(string s)
        {
            List<AphidType> values = Parser.TokenizeAndParse(s);
            values.ForEach(x => x.ToString());  // FIXME
        }
    }
}