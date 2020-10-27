using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LeafMachine.Aphid
{
    public class Tokenizer
    {
        public static List<string> Tokenize(string data)
        {
            List<string> tokens = new List<string>();
            char[] delimiters = { ' ', '\n', '\t', '\r' };
            string[] lines = data.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) {
                MatchCollection results = Regex.Matches(line, "\"[^\"]*\"|;.*?(\\n|$)|\\S+", RegexOptions.Multiline);
                string[] possibleTokens = results.Select(m => m.Value).ToArray();
                foreach (string s in possibleTokens) {
                    if (!s.StartsWith(';'))
                        tokens.Add(s);
                }
            }
            return tokens;
        }
    }
}