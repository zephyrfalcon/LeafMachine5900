using LeafMachine.Aphid.Types;
using System.Collections.Generic;

namespace LeafMachine
{
    public static class Expect
    {
        public static int ExpectInteger(string context, AphidType x)
        {
            if (x is AphidInteger) {
                return (x as AphidInteger).AsInteger();
            }
            else throw new System.Exception($"{context}: integer expected, got {x.ToString()} instead");
        }

        public static string ExpectString(string context, AphidType x)
        {
            if (x is AphidString) {
                return (x as AphidString).AsString();
            }
            else throw new System.Exception($"{context}: string expected, got {x.ToString()} instead");
        }

        public static AphidList ExpectAphidList(string context, AphidType x)
        {
            if (x is AphidList)
                return (x as AphidList);
            else throw new System.Exception($"{context}: list expected, got {x.ToString()} instead");
        }

    }
}