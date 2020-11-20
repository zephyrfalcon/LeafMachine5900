using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LeafMachine
{
    public static class KeyCodes
    {
        public static Dictionary<Keys, string> keys = new Dictionary<Keys, string> {
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
            { Keys.Escape, "esc" },
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
    }
}