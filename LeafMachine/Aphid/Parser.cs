using LeafMachine.Aphid.Types;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LeafMachine.Aphid
{
    public class Parser
    {
        static string re_integer = "\\d+";
        public static List<AphidType> Parse(string[] tokens)
        {
            List<AphidType> values = new List<AphidType>();
            foreach (string token in tokens)
            {
                Match match = Regex.Match(token, re_integer);
                if (match.Success)
                {
                    values.Add(new AphidInteger(Int32.Parse(token)));
                    continue;
                }

                // if nothing else matches, it's a symbol
                throw new Exception("to be implemented: symbol");
            }
            return values;
        }

        public static List<AphidType> TokenizeAndParse(string data)
        {
            string[] tokens = Tokenizer.Tokenize(data);
            return Parse(tokens);
        }
    }
}