using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;


namespace LeafMachine.Aphid
{
    public class ShortcutExpander
    {
        static string re_get_variable = @"^\$([^!]\S*)$";
        static string re_set_variable = @"^\$!(\S+)$";

        // If the token is a shortcut, return a list of tokens (strings) that it expands to.
        // Otherwise, return null.
        public List<string> Expand(string token)
        {
            Match match;
            match = Regex.Match(token, re_set_variable);
            if (match.Success) {
                // $!foo => :foo setvar
                return new List<string> { $":{match.Groups[1]}", "setvar" };
            }
            match = Regex.Match(token, re_get_variable);
            if (match.Success) {
                // $foo => :foo getvar
                return new List<string> { $":{match.Groups[1]}", "getvar" };
            }
            return null;
        }

        public List<string> ExpandTokens(List<string> tokens)
        {
            ShortcutExpander exp = new ShortcutExpander();
            List<string> expanded_tokens = new List<string> { };
            tokens.ForEach(token => {
                List<string> expanded = exp.Expand(token);
                if (expanded is null) {
                    expanded_tokens.Add(token);
                } else {
                    expanded_tokens.AddRange(expanded);
                }
            });
            return expanded_tokens;
        }
    }
}