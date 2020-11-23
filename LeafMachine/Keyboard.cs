using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace LeafMachine
{
    public class KeyCodes
    {
        private Dictionary<Keys, string> keys = new Dictionary<Keys, string> {
            // letters
            { Keys.A, "a" },
            { Keys.B, "b" },
            { Keys.C, "c" },
            { Keys.D, "d" },
            { Keys.E, "e" },
            { Keys.F, "f" },
            { Keys.G, "g" },
            { Keys.H, "h" },
            { Keys.I, "i" },
            { Keys.J, "j" },
            { Keys.K, "k" },
            { Keys.L, "l" },
            { Keys.M, "m" },
            { Keys.N, "n" },
            { Keys.O, "o" },
            { Keys.P, "p" },
            { Keys.Q, "q" },
            { Keys.R, "r" },
            { Keys.S, "s" },
            { Keys.T, "t" },
            { Keys.U, "u" },
            { Keys.V, "v" },
            { Keys.W, "w" },
            { Keys.X, "x" },
            { Keys.Y, "y" },
            { Keys.Z, "z" },
            // numbers
            { Keys.D0, "0" },
            { Keys.D1, "1" },
            { Keys.D2, "2" },
            { Keys.D3, "3" },
            { Keys.D4, "4" },
            { Keys.D5, "5" },
            { Keys.D6, "6" },
            { Keys.D7, "7" },
            { Keys.D8, "8" },
            { Keys.D9, "9" },
            // non-alphanumeric
            { Keys.Space, "space" },
            { Keys.OemComma, "comma" },
            { Keys.OemPeriod, "period" },
            { Keys.OemPlus, "plus" },
            { Keys.OemMinus, "minus" },
            { Keys.OemSemicolon, "semicolon" },
            { Keys.OemBackslash, "backslash" },
            { Keys.OemOpenBrackets, "left-bracket" },
            { Keys.OemCloseBrackets, "right-bracket" },
            { Keys.OemPipe, "pipe" },
            { Keys.OemQuestion, "question-mark" },
            { Keys.OemTilde, "tilde" },
            // cursor
            { Keys.Left, "cursor-left" },
            { Keys.Right, "cursor-right" },
            { Keys.Up, "cursor-up" },
            { Keys.Down, "cursor-down" },
            // other
            { Keys.Escape, "escape" },
            { Keys.Back, "backspace" },
            { Keys.Delete, "delete" },
            { Keys.Insert, "insert" },
            { Keys.Home, "home" },
            { Keys.End, "end" },
            { Keys.PageUp, "pgup" },
            { Keys.PageDown, "pgdn" },
            { Keys.Enter, "enter" },
            // modifier keys
            { Keys.LeftShift, "left-shift" },
            { Keys.LeftControl, "left-ctrl" },
            { Keys.LeftAlt, "left-alt" },
            { Keys.LeftWindows, "left-windows" },
        };

        private Dictionary<string, Keys> symbols;

        public KeyCodes()
        {
            symbols = new Dictionary<string, Keys> { };
            foreach(KeyValuePair<Keys,string> kv in keys) {
                symbols.Add(kv.Value, kv.Key);
            }
        }

        public string KeyToName(Keys k)
        {
            return keys[k];
        }

        public Keys NameToKey(string name)
        {
            return symbols[name];
        }
    }

    public class KeyboardHandler

        // NOTE: VERY MUCH A WORK IN PROGRESS.
    {
        long tix = 0;
        // when we press and hold a key...
        int repeatWait = 25;  // it takes this many tix to start repeating
        int repeatRate = 6;  // after the first repeat, the keys will show up after this many tix
        int repeatCoolDown = 30;  // during a repeat, if this many tix pass, the repeat is considered to be finished
        Keys lastKeyPressed;
        int tixSinceInitialKeypress = 0;
        int tixSinceLastKeypress = 0;
        bool isInitialKeyPress = false;
        bool debug = false;

        public KeyboardHandler()
        {
        }

        public void Update()
        {
            tix++;
            if (isInitialKeyPress) tixSinceInitialKeypress++;
            else tixSinceLastKeypress++;
        }

        public void ResetRepeatState(Keys newKey)
        {
            lastKeyPressed = newKey;
            isInitialKeyPress = true;
            tixSinceInitialKeypress = 0;
            tixSinceLastKeypress = 0;
        }

        // we need something more, like the equivalent of "GetKey" or w/e, which returns a key (or
        // multiple keys) that have been pressed
        // probably based on Keyboard.GetState().GetPressedKeys()
        // we also need a way to combine modifier keys with "regular" keys... and consider them
        // "one key combination"

        // appears to work, but does not take modifier keys (Shift, Ctrl, etc) into account, since those
        // are considered different keys
        public bool HasBeenPressed(Keys key)
        {
            bool keyDown = Keyboard.GetState().IsKeyDown(key);
            if (keyDown) {
                if (debug) Console.WriteLine($"Was {key} pressed? {keyDown}");
                // key was not pressed in the previous 'tick'
                if (key == lastKeyPressed) {
                    if (debug) Console.WriteLine($"Key {key} is lastKeyPressed! {lastKeyPressed}");
                    if (isInitialKeyPress) {
                        if (debug) Console.WriteLine("An initial key press!");
                        if (tixSinceInitialKeypress > repeatWait) {
                            if (debug) Console.WriteLine($"Enough time has passed for the initial wait! {tixSinceInitialKeypress}");
                            // enough time has passed since the previous, initial keypress
                            isInitialKeyPress = false;
                            tixSinceLastKeypress = 0;  // start counting
                            return true;
                        }
                        else {
                            if (debug) Console.WriteLine($"Not enough time has passed for the initial wait! {tixSinceLastKeypress}");
                            // not enough time has passed since the previous, initial keypress
                            return false;
                        }
                    }
                    else {
                        // this is a repeat
                        if (tixSinceLastKeypress > repeatCoolDown) {
                            // we've waited too long between keypresses, this will be treated like a new keypress
                            ResetRepeatState(key);
                            return true;
                        }
                        if (tixSinceLastKeypress > repeatRate) {
                            if (debug) Console.WriteLine($"Enough time has passed since the last repeat! {tixSinceLastKeypress}");
                            // enough time has passed since the last keypress
                            tixSinceLastKeypress = 0;  // reset repeat counter
                            return true;
                        }
                        else {
                            if (debug) Console.WriteLine($"Not enough time has passed since the last repeat! {tixSinceLastKeypress}");
                            // not enough time has passed since the last keypress
                            return false;
                        }
                    }
                }
                else {
                    // this is not the same key that was last pressed
                    // all 'repeat' state gets reset
                    ResetRepeatState(key);
                    return true;
                }
            }
            else {
                return false;
            }
        }
    }

}