using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LeafMachine.CharSets
{
    // NOTE: At this point, CharSets are meant for *hires characters* only!
    // I still need to figure out what to do with multicolor characters... The principe is the same,
    // but the bitmaps aren't.

    abstract public class CharSet
    {
        // the actual bitmaps as int[64] arrays (values must be 0 or 1). 
        // array size may differ per charset.
        protected int[,] bitmaps64;

        // maps characters ("a") or names ("arrow-up") to index in bitmaps64
        protected Dictionary<string, int> charToBitmapIndex;

        public virtual int[,] InitBitmaps()
        {
            throw new System.Exception("not implemented");
        }

        public virtual Dictionary<string, int> CharToBitmapIndex()
        {
            throw new System.Exception("not implemented");
        }

        public CharSet()
        {
            bitmaps64 = InitBitmaps();
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