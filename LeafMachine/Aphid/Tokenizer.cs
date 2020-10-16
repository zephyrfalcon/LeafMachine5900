using System.Collections.Generic;

namespace LeafMachine.Aphid
{
    public class Tokenizer
    {
        // very basic version for now; simply splits the string on whitespace; does not take
        // into account spaces inside string literals, comments, etc.
        public static string[] Tokenize(string data)
        {
            List<string> tokens = new List<string>();
            char[] delimiters = { ' ', '\n', '\t', '\r' };
            string[] lines = data.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) {
                string[] parts = line.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts) {
                    tokens.Add(part);
                }
            }
            return tokens.ToArray();
        }
    }
}