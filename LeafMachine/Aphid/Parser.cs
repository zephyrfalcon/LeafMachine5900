using LeafMachine.Aphid.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LeafMachine.Aphid
{
    public class Parser
    {
        static string re_integer = "^-?\\d+$";
        static string re_string = "^\".*?\"$";

        public static List<AphidType> Parse(List<string> tokens)
        {
            List<List<AphidType>> lists = new List<List<AphidType>> { new List<AphidType> { } };
            //List<AphidType> values = new List<AphidType>();
            tokens.ForEach(token => {
                //foreach (string token in tokens) {
                if (token == "{") {
                    // we are starting a new block, so we add a new list to `lists`
                    lists.Add(new List<AphidType> { });
                }
                else if (token == "}") {
                    // having <= 1 elements in `lists` is an error at this point.
                    if (lists.Count <= 1)
                        throw new Exception("parser error");
                    // a block just ended; we take the topmost list from `lists`, and turn it into
                    // an AphidBlock, and push that onto the new topmost list.
                    List<AphidType> topmost = lists.ElementAt(lists.Count - 1);
                    lists.RemoveAt(lists.Count - 1);
                    AphidBlock blk = new AphidBlock(topmost);
                    lists.ElementAt(lists.Count - 1).Add(blk);
                }
                else {
                    ParseToken(token, lists.ElementAt(lists.Count - 1));
                }
            });

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
            match = Regex.Match(token, re_string);
            if (match.Success) {
                list.Add(new AphidString(token.Substring(1, token.Length - 2)));
                return;
            }

            // if nothing else matches, it's a symbol
            list.Add(new AphidSymbol(token));
        }

        public static List<AphidType> TokenizeAndParse(string data)
        {
            List<string> tokens = Tokenizer.Tokenize(data);
            List<string> expanded_tokens = new ShortcutExpander().ExpandTokens(tokens);
            return Parse(expanded_tokens);
        }
    }
}