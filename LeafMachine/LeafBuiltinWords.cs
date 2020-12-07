﻿using System;
using System.Collections.Generic;
using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;
using LeafMachine.CharSets;
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

        public void SetChar(AphidInterpreter aip, MachineState state)
        {
            // ( bitmap charname charset -- )
            string charset = Expect.ExpectSymbol("set-char", aip.stack.Pop());
            string charname = Expect.ExpectString("set-char", aip.stack.Pop());
            AphidList alist = Expect.ExpectAphidList("set-char", aip.stack.Pop());
            List<AphidType> list = alist.AsList();
            if (list.Count != 64)
                throw new Exception($"set-char: bitmap must have 64 elements; got {list.Count} instead");
            int[] bitmap = new int[64];
            for (int i = 0; i < 64; i++) {
                if (list[i] is AphidInteger) {
                    bitmap[i] = (list[i] as AphidInteger).AsInteger();
                    if (bitmap[i] != 0 && bitmap[i] != 1)
                        throw new Exception("set-char: bitmap must consist of 0 and 1 only; got {bitmap[i]}");
                }
                else throw new Exception($"set-char: bitmap must consist of integers only; found {list[i]}");
            }
            GraphicCharSet gcs = state.gcsmanager.GetCharSet(charset);
            CharSet cs = gcs.GetCharSet();
            if (cs is CustomCharSet) {
                CustomCharSet ccs = (cs as CustomCharSet);
                ccs.AddBitmap(charname, bitmap);
                gcs.AddGraphicChar(charname);
            }
            else throw new Exception("charset is not a CustomCharSet"); // FIXME later?
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
                { "set-char", SetChar },
            };
            return bw;
        }

    }
}