using System.Collections.Generic;
using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;

namespace LeafMachine
{
    public class LeafBuiltinWords
    {
        public void WriteXY(AphidInterpreter aip, MachineState state)
        {
            // writexy ( x y color text -- )
            AphidType atext = aip.stack.Pop();
            AphidType acolor = aip.stack.Pop();  // color index
            AphidType ay = aip.stack.Pop();
            AphidType ax = aip.stack.Pop();

            // lots of busywork here...

            string text = "";
            int color, x, y;

            if (atext is AphidString) {
                text = (atext as AphidString).AsString();
            }
            else throw new System.Exception($"writexy: text must be string, got {atext.ToString()} instead");

            if (acolor is AphidInteger) {
                color = (acolor as AphidInteger).AsInteger();
                if (color < 1 || color > MachineState.NUM_COLORS)
                    throw new System.Exception($"writexy: color must be a value between 1 and 16; got {color} instead");
            }
            else throw new System.Exception($"writexy: color must be integer 1..16, got {acolor.ToString()} instead");

            if (ax is AphidInteger) {
                x = (ax as AphidInteger).AsInteger();
                if (x < 0 || x >= MachineState.WIDTH)
                    throw new System.Exception($"writexy: x must be between 0 and {MachineState.WIDTH-1}");
            }
            else throw new System.Exception($"writexy: x must be an integer; got {ax.ToString()} instead");

            if (ay is AphidInteger) {
                y = (ay as AphidInteger).AsInteger();
                if (y < 0 || y >= MachineState.HEIGHT)
                    throw new System.Exception($"writexy: y must be between 0 and {MachineState.HEIGHT - 1}");
            }
            else throw new System.Exception($"writexy: y must be an integer; got {ay.ToString()} instead");

            // finally, set the values in MachineState.chars
            for (int i=0; i < text.Length; i++) {
                int realX = i + x;
                if (realX < 0 || realX >= MachineState.WIDTH)
                    throw new System.Exception($"writexy: string too long to be displayed");
                state.SetChar(realX, y, text[i]);
                state.SetColor(realX, y, color);
            }
        }

        /* built-in words */

        public Dictionary<string, DelAphidLeafBuiltinWord> GetBuiltinWords()
        {
            Dictionary<string, DelAphidLeafBuiltinWord> bw = new Dictionary<string, DelAphidLeafBuiltinWord> {
                { "writexy", WriteXY },
            };
            return bw;
        }

    }
}