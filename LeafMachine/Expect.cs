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

        public static int ExpectColor(string context, AphidType x)
        {
            int color = ExpectInteger(context, x);
            if (color > 0 && color <= MachineState.NUM_COLORS) {
                return color;
            }
            else throw new System.Exception($"{context}: color must be a value between 1 and {MachineState.NUM_COLORS}; got {color} instead");
        }

        // TODO:
        // ExpectX, ExpectY
        // ExpectSymbol
        // ExpectBlock
    }
}