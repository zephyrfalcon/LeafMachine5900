using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LeafMachine.Aphid
{
    public class Tokenizer
    {
        // rather basic version for now; simply splits the string on whitespace; does not take
        // into account spaces inside string literals, etc.
        public static List<string> Tokenize(string data)
        {
            List<string> tokens = new List<string>();
            char[] delimiters = { ' ', '\n', '\t', '\r' };
            string[] lines = data.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) {
                MatchCollection results = Regex.Matches(line, "\"[^\"]*\"|;.*?(\\n|$)|\\S+", RegexOptions.Multiline);
                string[] blah = results.Select(m => m.Value).ToArray();
                foreach (string s in blah) {
                    if (!s.StartsWith(';'))
                        tokens.Add(s);
                }

                /*
                string[] parts = line.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts) {
                    // if we encounter a free-standing semicolon, then that plus all the text that follows
                    // on that line, are considered a comment, and consequently ignored
                    if (part == ";")
                        break;
                    tokens.Add(part);
                }
                */
            }
            return tokens;
        }
    }
}