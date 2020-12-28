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

        public static int ExpectColorOr0(string context, AphidType x)
        {
            int color = ExpectInteger(context, x);
            if (color >= 0 && color <= MachineState.NUM_COLORS) {
                return color;
            }
            else throw new System.Exception($"{context}: color must be a value between 0 and {MachineState.NUM_COLORS}; got {color} instead");
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

        public static int[] ExpectBitmap64(string context, AphidType x)
        {
            if (!(x is AphidList)) 
                throw new System.Exception($"{context}: list expected, got {x.ToString()} instead");
            AphidList alist = (x as AphidList);
            List<AphidType> values = alist.AsList();
            if (values.Count != 64)
                throw new System.Exception($"{context}: bitmap must have exactly 64 elements; got {values.Count} instead");
            int[] bits = new int[64];
            for (int i = 0; i < 64; i++) {
                AphidType a = values[i];
                if (!(a is AphidInteger))
                    throw new System.Exception($"{context}: bitmap must contain integers; got {a.ToString()} instead");
                AphidInteger ai = (a as AphidInteger);
                int bit = ai.AsInteger();
                if (!(bit == 0 || bit == 1))
                    throw new System.Exception($"{context}: bitmap can only have 0 and 1; got {bit} instead");
                bits[i] = bit;
            }
            return bits;
        }

        public static int[] ExpectBitmap32(string context, AphidType x)
        {
            if (!(x is AphidList))
                throw new System.Exception($"{context}: list expected, got {x.ToString()} instead");
            AphidList alist = (x as AphidList);
            List<AphidType> values = alist.AsList();
            if (values.Count != 32)
                throw new System.Exception($"{context}: bitmap must have exactly 32 elements; got {values.Count} instead");
            int[] colors = new int[32];
            for (int i = 0; i < 32; i++) {
                AphidType a = values[i];
                if (!(a is AphidInteger))
                    throw new System.Exception($"{context}: bitmap must contain integers; got {a.ToString()} instead");
                AphidInteger ai = (a as AphidInteger);
                int color = ai.AsInteger();
                if (color < 0 || color > MachineState.NUM_COLORS)
                    throw new System.Exception($"{context}: multicolor bitmap can have values of 0..{MachineState.NUM_COLORS}; got {color} instead");
                colors[i] = color;
            }
            return colors;
        }


        // TODO:
        // ExpectX, ExpectY
    }
}