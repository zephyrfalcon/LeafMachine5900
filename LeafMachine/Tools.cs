using LeafMachine;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LeafMachine
{
    public class Tools {
        public string ExecutablePath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

    }

    public static class StringReverser
    {
        private static IEnumerable<string> GraphemeClusters(this string s)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(s);
            while (enumerator.MoveNext()) {
                yield return (string)enumerator.Current;
            }
        }
        public static string ReverseGraphemeClusters(this string s)
        {
            return string.Join("", s.GraphemeClusters().Reverse().ToArray());
        }

    }
}