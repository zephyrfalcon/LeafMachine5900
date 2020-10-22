using Microsoft.Xna.Framework.Graphics;

namespace LeafMachine
{
    abstract public class GraphicChar
    {

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

    public class HiresChar : GraphicChar
    {
        private Texture2D image;
    }
}