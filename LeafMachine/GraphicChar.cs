using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Net;

namespace LeafMachine
{
    abstract public class GraphicChar
    {
        public virtual Texture2D GetImage() 
        {
            throw new System.Exception("not implemented");
        }
        // subject to change later, depending on type of GraphicChar...
    }

    /* It's not clear yet what these different classes actually *mean*.
     * I mean, we could have a HiresChar, a class for multicolor graphics, chars that actually
     * holds multiple chars and display something different depending on the time passed or
     * some other state, etc. Question is, do they need to *do* anything more than hold
     * these images? The actual plotting will probably be handled differently for each, so 
     * it might make no sense to have a Plot() method or something. (E.g. for a hires char,
     * this would require one foreground color... which should *also* be stored in MachineState
     * by the way... but for multicolor, we should just plot it with no extra params required.
     * A "spinner" OTOH would require some kind of integer to determine which image we need.
     * Needs more thought! */

    /* What we probably *do* need is different constructors for each. Like HiresChar could take
     * an int[64] array, which would then be translated to foreground and background colors (sort of,
     * see the "demo". */

    /* HiresChar:
     * Built from an 8x8 array of bits (ints of 0 or 1 really). 
     * When we plot this character, we specify a foreground color; the bits that have 1, will
     * show up as that color; the bits that have 0, are considered transparent and will show
     * up as the background color. 
     */
    public class HiresChar : GraphicChar
    {
        private Texture2D image;
        static Color fg = new Color(255, 255, 255, 255);
        static Color bg = new Color(0, 0, 0, 0);
        public HiresChar(GraphicsDeviceManager graphics, int[] bits)
        {
            image = new Texture2D(graphics.GraphicsDevice, 8, 8);
            Color[] colors = new Color[8 * 8];
            for (int i=0; i < 8*8; i++) {
                colors[i] = (bits[i] == 1) ? fg : bg;
            }
            image.SetData(colors);
        }
        public override Texture2D GetImage() { return image; }
    }

    public class MultiColorChar : GraphicChar
    {
        private Texture2D image;
        
        public MultiColorChar(GraphicsDeviceManager graphics, int[] flatpixels)
        {
            // there are 32 "flatpixels", each an integer value referring to the palette
            // as set in MachineState; a value of 0 means transparent.
            // MISSING: exactly this palette.
        }

        public override Texture2D GetImage() { return image; }
    }
}
