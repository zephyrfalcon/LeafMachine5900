using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LeafMachine.CharSets
{
    class CustomCharSet: CharSet
    {
        protected int size;
        protected int currIndex = 0;

        public CustomCharSet(int thesize)
        {
            size = thesize;
            currIndex = 0;
            bitmaps64 = InitBitmaps();
            charToBitmapIndex = CharToBitmapIndex();
        }

        public override int[,] InitBitmaps()
        {
            int[,] bitmaps64 = new int[size,64];
            return bitmaps64;
        }

        public override Dictionary<string, int> CharToBitmapIndex()
        {
            return new Dictionary<string, int> { };
        }

        public void AddBitmap(string name, int[] bitmap)
        {
            if (currIndex >= size)
                throw new Exception("CharSet is full");
            if (bitmap.Length != 64)
                throw new Exception($"Wrong size of bitmap; must be 64 values; got {bitmap.Length} instead");
            // clumsy C# array handling requires us to add each "bit" separately
            for (int i = 0; i < 64; i++) {
                bitmaps64[currIndex,i] = bitmap[i];
            }
            charToBitmapIndex[name] = currIndex;
            currIndex++;
        }

    }
}