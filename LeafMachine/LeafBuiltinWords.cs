using System;
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
            // Plot the given character at (x,y) in the given color, using the current charset.
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
            // Set the default charset, where characters/names will be looked up if they
            // are not found in the current charset.
            string name = Expect.ExpectSymbol("set-default-charset", aip.stack.Pop());
            // check if this a known charset name and complain if it isn't
            if (!state.gcsmanager.KnownCharSetNames().Contains(name))
                throw new Exception($"set-default-charset: Unknown charset name: {name}");
            state.defaultCharSet = name;
        }

        public void SetCurrentCharset(AphidInterpreter aip, MachineState state)
        {
            // ( :name -- )
            // Set the current charset, i.e. the charset that will be used by setxy/writexy if we don't
            // specify anything else.
            string name = Expect.ExpectSymbol("set-current-charset", aip.stack.Pop());
            // check if this a known charset name and complain if it isn't
            if (!state.gcsmanager.KnownCharSetNames().Contains(name))
                throw new Exception($"set-current-charset: Unknown charset name: {name}");
            state.currentCharSet = name;
        }

        public void CurrentCharSet(AphidInterpreter aip, MachineState state)
        {
            // ( -- :charset )
            // Push the name of the current charset, as a symbol.
            aip.stack.Push(new AphidSymbol(state.currentCharSet));
        }

        public void SetFG(AphidInterpreter aip, MachineState state)
        {
            // ( x y color -- )
            // Set the foreground color for the given screen position.
            int color = Expect.ExpectColor("setfg", aip.stack.Pop());
            int y = Expect.ExpectYCoordinate("setfg", aip.stack.Pop());
            int x = Expect.ExpectXCoordinate("setfg", aip.stack.Pop());
            state.chars[x, y].fgcolor = color;
        }

        public void GetFG(AphidInterpreter aip, MachineState state)
        {
            // ( x y -- color )
            // Get the foreground color for the given screen position.
            int y = Expect.ExpectYCoordinate("getfg", aip.stack.Pop());
            int x = Expect.ExpectXCoordinate("getfg", aip.stack.Pop());
            aip.stack.Push(new AphidInteger(state.chars[x, y].fgcolor));
        }

        // TODO: refactor out code that deals with expecting a bitmap
        public void OBSOLETE_SetChar(AphidInterpreter aip, MachineState state)
        {
            // FIXME: deals with hires chars only, so rename or rework
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
                ccs.AddBitmap(charname, new HiresCharBitmap(bitmap));
                gcs.AddGraphicChar(charname);
            }
            else throw new Exception("charset is not a CustomCharSet"); // FIXME later?
        }

        public void GetHiresBitmap(AphidInterpreter aip, MachineState state)
        {
            // ( charset charname -- [bitmap64...] )
            string charname = Expect.ExpectString("get-hires-bitmap", aip.stack.Pop());
            string charset = Expect.ExpectSymbol("get-hires-bitmap", aip.stack.Pop());
            CharBitmap bitmap = state.gcsmanager.GetCharSet(charset).GetCharSet().BitmapForChar(charname);
            if (bitmap is HiresCharBitmap) {
                int[] bits = (bitmap as HiresCharBitmap).GetValues();
                // ^ might have to pull this apart to allow for clearer error messages?
                List<AphidType> abits = new List<AphidType>();
                for (int i = 0; i < 64; i++) {
                    abits.Add(new AphidInteger(bits[i]));
                }
                aip.stack.Push(new AphidList(abits));
            }
            else throw new Exception($"get-hires-bitmap: requires hires bitmap");
        }

        public void GetMultiColorBitmap(AphidInterpreter aip, MachineState state)
        {
            // ( charset charname -- [bitmap32...] )
            string charname = Expect.ExpectString("get-hires-bitmap", aip.stack.Pop());
            string charset = Expect.ExpectSymbol("get-hires-bitmap", aip.stack.Pop());
            CharBitmap bitmap = state.gcsmanager.GetCharSet(charset).GetCharSet().BitmapForChar(charname);
            if (bitmap is MultiColorCharBitmap) {
                int[] colors = (bitmap as MultiColorCharBitmap).GetValues();
                // ^ might have to pull this apart to allow for clearer error messages?
                List<AphidType> acolors = new List<AphidType>();
                for (int i = 0; i < 32; i++) {
                    acolors.Add(new AphidInteger(colors[i]));
                }
                aip.stack.Push(new AphidList(acolors));
            }
            else throw new Exception($"get-multicolor-bitmap: requires multicolor bitmap");
        }

        public void SetHiresBitmap(AphidInterpreter aip, MachineState state)
        {
            // ( charset charname bitmap -- )
            // Q: Shouldn't the order be: bitmap charset charname? When you define a bitmap "manually",
            // the list should really come first... like we define words...
            int[] bits = Expect.ExpectBitmap64("set-hires-bitmap", aip.stack.Pop());
            string charname = Expect.ExpectString("set-hires-bitmap", aip.stack.Pop());
            string charset = Expect.ExpectSymbol("set-hires-bitmap", aip.stack.Pop());
            CharSet cs = state.gcsmanager.GetCharSet(charset).GetCharSet();
            // only a CustomCharSet can (currently) add a bitmap
            // AtariCharSet, C64CharSet etc, are "set in stone"
            if (cs is CustomCharSet) {
                CustomCharSet ccs = cs as CustomCharSet;
                ccs.AddBitmap(charname, new HiresCharBitmap(bits));
            }
            else throw new Exception("set-hires-bitmap: CharSet is not a CustomCharSet");
            // add to the appropriate GraphicCharSet to create the Texture2D
            state.gcsmanager.GetCharSet(charset).AddGraphicChar(charname);
        }

        public void SetMultiColorBitmap(AphidInterpreter aip, MachineState state)
        {
            // ( charset charname bitmap -- )
            int[] bitmap32 = Expect.ExpectBitmap32("set-multicolor-bitmap", aip.stack.Pop());
            string charname = Expect.ExpectString("set-multicolor-bitmap", aip.stack.Pop());
            string charset = Expect.ExpectSymbol("set-multicolor-bitmap", aip.stack.Pop());
            CharSet cs = state.gcsmanager.GetCharSet(charset).GetCharSet();
            if (cs is CustomCharSet) {
                CustomCharSet ccs = cs as CustomCharSet;
                ccs.AddBitmap(charname, new MultiColorCharBitmap(bitmap32));
            }
            else throw new Exception("set-multicolor-bitmap: CharSet is not a CustomCharSet");
            state.gcsmanager.GetCharSet(charset).AddGraphicChar(charname);
        }

        public void GetCharnames(AphidInterpreter aip, MachineState state)
        {
            // ( charset -- [charnames...] )
            string charset = Expect.ExpectSymbol("get-charnames", aip.stack.Pop());
            string[] charnames = state.gcsmanager.GetCharSet(charset).GetCharSet().KnownChars();
            List<AphidType> names = new List<AphidType>();
            for (int i = 0; i < charnames.Length; i++) {
                names.Add(new AphidString(charnames[i]));
            }
            aip.stack.Push(new AphidList(names));
        }

        public void MakeCharset(AphidInterpreter aip, MachineState state)
        {
            // ( :name -- )
            // Make a new charset (CustomCharSet) and register it under the given name. 
            // Charset starts out empty.
            string charset = Expect.ExpectSymbol("make-charset", aip.stack.Pop());
            CustomCharSet cs = new CustomCharSet(1024);
            GraphicCharSet gcs = new GraphicCharSet(state, cs, charset);
            state.gcsmanager.Add(charset, gcs);
        }

        public void Debug(AphidInterpreter aip, MachineState state)
        {
            // ( -- )
            // Display debugging info.
            Console.WriteLine($"DEBUG: # {aip.stack.SimpleRepr()}");
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
                { "set-current-charset", SetCurrentCharset },
                //{ "set-char", SetChar },
                { "current-charset", CurrentCharSet },
                { "setfg", SetFG },
                { "getfg", GetFG },
                { "get-hires-bitmap", GetHiresBitmap },
                { "get-multicolor-bitmap", GetMultiColorBitmap },
                { "get-charnames", GetCharnames },
                { "make-charset", MakeCharset },
                { "set-hires-bitmap", SetHiresBitmap },
                { "set-multicolor-bitmap", SetMultiColorBitmap },
                { "debug", Debug },
            };
            return bw;
        }

    }
}