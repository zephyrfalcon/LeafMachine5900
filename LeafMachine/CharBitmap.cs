namespace LeafMachine
{
    /* A CharBitmap is really just a wrapper around int[], except we have different versions
       for hires (64 bits) and multicolor (32 color values). 
       Using arrays of CharBitmap elsewhere allows us to mix hires and multicolor bitmaps
       unambiguously.
    */

    public abstract class CharBitmap
    {
        public virtual int[] GetValues()
        {
            throw new System.Exception("not implemented");
        }

        public virtual GraphicChar MakeChar(MachineState state)
        {
            throw new System.Exception("not implemented");
        }
    }

    public class HiresCharBitmap : CharBitmap
    {
        int[] bitmap64;
        public HiresCharBitmap(int[] bits)
        {
            if (bits.Length != 64) throw new System.Exception($"number of bits must be exactly 64, got {bits.Length}");
            bitmap64 = bits;
        }
        public override int[] GetValues() { return bitmap64; }
        public override GraphicChar MakeChar(MachineState state)
        {
            return new HiresChar(state, this);
        }
    }

    public class MultiColorCharBitmap : CharBitmap
    {
        int[] bitmap32;
        public MultiColorCharBitmap(int[] values)
        {
            if (values.Length != 32) throw new System.Exception($"number of values must be exactly 32, got {values.Length}");
            bitmap32 = values;
        }
        public override int[] GetValues() { return bitmap32; }
        public override GraphicChar MakeChar(MachineState state)
        {
            return new MultiColorChar(state, this);
        }
    }
}
