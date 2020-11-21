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

        public static int ExpectXCoordinate(string context, AphidType thing)
        {
            int x = ExpectInteger(context, thing);
            if (x >= 0 && x < MachineState.WIDTH) {
                return x;
            }
            else throw new System.Exception($"{context}: X-coordinate must be a value between 0 and {MachineState.WIDTH}; got {x} instead");
        }


        public static int ExpectYCoordinate(string context, AphidType x)
        {
            int y = ExpectInteger(context, x);
            if (y >= 0 && y < MachineState.HEIGHT) {
                return y;
            }
            else throw new System.Exception($"{context}: Y-coordinate must be a value between 0 and {MachineState.HEIGHT}; got {y} instead");
        }

        public static string ExpectSymbol(string context, AphidType x)
        {
            if (x is AphidSymbol) {
                return (x as AphidSymbol).GetValue();
            }
            else throw new System.Exception($"{context}: symbol expected, got {x.ToString()} instead");
        }

        public static AphidBlock ExpectAphidBlock(string context, AphidType x)
        {
            if (x is AphidBlock) {
                return (x as AphidBlock);
            }
            else throw new System.Exception($"{context}: block expected, got {x.ToString()} instead");
        }

        public static bool ExpectBool(string context, AphidType x)
        {
            if (x is AphidBool)
                return (x as AphidBool).IsTrue();
            else throw new System.Exception($"{context}: bool expected, got {x.ToString()} instead");
        }


        // TODO:
        // ExpectX, ExpectY
    }
}