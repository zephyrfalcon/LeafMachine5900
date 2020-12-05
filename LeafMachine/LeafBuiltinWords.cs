using System;
using System.Collections.Generic;
using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;
using Microsoft.Xna.Framework.Input;

namespace LeafMachine
{
    public class LeafBuiltinWords
    {
        public void WriteXY(AphidInterpreter aip, MachineState state)
        {
            // writexy ( x y color text -- )
            // Write the given text, in the default charset, starting at position (x, y).
            string text = "";
            int color, x, y;

            text = Expect.ExpectString("writexy", aip.stack.Pop());
            color = Expect.ExpectColor("writexy", aip.stack.Pop());
            y = Expect.ExpectYCoordinate("writexy", aip.stack.Pop());
            x = Expect.ExpectXCoordinate("writexy", aip.stack.Pop());

            // finally, set the values in MachineState.chars
            for (int i=0; i < text.Length; i++) {
                int realX = i + x;
                if (realX < 0 || realX >= MachineState.WIDTH)
                    throw new System.Exception($"writexy: string too long to be displayed");
                state.SetChar(realX, y, text[i].ToString());
                state.SetColor(realX, y, color);
            }
        }

        public void SetXY(AphidInterpreter aip, MachineState state)
        {
            // ( x y fgcolor charname -- )
            // Plot the given character at (x,y) in the given color.
            // Uses the default charset.
            string charname = Expect.ExpectString("setxy", aip.stack.Pop());
            int fgcolor = Expect.ExpectColor("setxy", aip.stack.Pop());
            int y = Expect.ExpectYCoordinate("setxy", aip.stack.Pop());
            int x = Expect.ExpectXCoordinate("setxy", aip.stack.Pop());

            state.SetChar(x, y, charname);
            state.SetColor(x, y, fgcolor);
        }

        public void SetBG(AphidInterpreter aip, MachineState state)
        {
            // ( color -- )
            int color = Expect.ExpectColor("setbg", aip.stack.Pop());
            state.bgColor = color;
        }

        public void GetBG(AphidInterpreter aip, MachineState state)
        {
            // ( -- color )
            aip.stack.Push(new AphidInteger(state.bgColor));
        }

        public void Tix(AphidInterpreter aip, MachineState state)
        {
            // ( -- tix )
            aip.stack.Push(new AphidInteger(state.tix));
            // TODO: instead of endlessly creating new AphidInteger objects for this, we *could* just
            // reuse an existing one... from Aphid's POV they are immutable anyway.
        }

        public void SetUpdater(AphidInterpreter aip, MachineState state)
        {
            // ( name -- )
            string name = Expect.ExpectSymbol("set-updater", aip.stack.Pop());
            AphidWord word = aip.Lookup(name);
            state.SetUpdater(word);
        }

        public void KeyDown(AphidInterpreter aip, MachineState state)
        {
            // ( symbol -- bool )
            // Returns true if the given key (represented as a symbol :#foo) is down, false otherwise
            string name = Expect.ExpectSymbol("key-down?", aip.stack.Pop());
            if (name.StartsWith('#')) 
                name = name.Substring(1);
            Keys k = state.keycodes.NameToKey(name);
            aip.stack.Push(new AphidBool(state.kbhandler.HasBeenPressed(k)));
        }

        public void SetDefaultCharset(AphidInterpreter aip, MachineState state)
        {
            // ( :name -- )
            string name = Expect.ExpectSymbol("set-default-charset", aip.stack.Pop());
            // XXX TODO: check if this a known charset name and complain if it isn't
            if (!state.gcsmanager.KnownCharSetNames().Contains(name))
                throw new Exception($"set-default-charset: Unknown charset name: {name}");
            state.defaultCharSet = name;
        }

        /* built-in words */

        public Dictionary<string, DelAphidLeafBuiltinWord> GetBuiltinWords()
        {
            Dictionary<string, DelAphidLeafBuiltinWord> bw = new Dictionary<string, DelAphidLeafBuiltinWord> {
                { "writexy", WriteXY },
                { "setxy", SetXY },
                { "setbg", SetBG },
                { "getbg", GetBG },
                { "tix", Tix },
                { "set-updater", SetUpdater },
                { "key-down?", KeyDown },
                { "set-default-charset", SetDefaultCharset },
            };
            return bw;
        }

    }
}