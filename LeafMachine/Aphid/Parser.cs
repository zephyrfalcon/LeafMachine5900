using LeafMachine.Aphid.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text.RegularExpressions;

namespace LeafMachine.Aphid
{
    public class Parser
    {
        static string re_integer = "\\d+";

        public static List<AphidType> Parse(string[] tokens)
        {
            List<List<AphidType>> lists = new List<List<AphidType>> { new List<AphidType> { } };
            //List<AphidType> values = new List<AphidType>();
            foreach (string token in tokens) {
                if (token == "{") {

                }
                else if (token == "}") {

                }
                else {
                    ParseToken(token, lists.ElementAt(lists.Count - 1));
                }
            }

            // at this point, we should have exactly one list in `lists`
            if (lists.Count == 1)
                return lists[0];
            else throw new Exception("parser error");
        }

        private static void ParseToken(string token, List<AphidType> list)
        {
            Match match = Regex.Match(token, re_integer);
            if (match.Success) {
                list.Add(new AphidInteger(Int32.Parse(token)));
                return;
            }

            // if nothing else matches, it's a symbol
            list.Add(new AphidSymbol(token));
        }

        public static List<AphidType> TokenizeAndParse(string data)
        {
            string[] tokens = Tokenizer.Tokenize(data);
            return Parse(tokens);
        }
    }
}