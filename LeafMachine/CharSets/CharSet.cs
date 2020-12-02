using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LeafMachine.CharSets
{
    abstract public class CharSet
    {
        protected int[,] bitmaps64;
        protected Dictionary<string, int> charToBitmapIndex;

        public virtual int[,] GetBitmaps()
        {
            throw new System.Exception("not implemented");
        }

        public virtual Dictionary<string, int> CharToBitmapIndex()
        {
            throw new System.Exception("not implemented");
        }

        public CharSet()
        {
            bitmaps64 = GetBitmaps();
            charToBitmapIndex = CharToBitmapIndex();
        }

        public int[] BitmapForIndex(int idx)
        {
            // apparently we cannot return a "nested" array, so I'll have to make one
            int[] bits = new int[64];
            for (int i = 0; i < 64; i++) {
                bits[i] = bitmaps64[idx, i];
            }
            return bits;
        }

        public int[] BitmapForChar(string name)
        {
            int idx = charToBitmapIndex[name];
            return BitmapForIndex(idx);
        }

        public string[] KnownChars()
        {
            return charToBitmapIndex.Keys.ToArray();
        }

    }

}