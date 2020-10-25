using Microsoft.Xna.Framework;
using LeafMachine;
using System.Runtime.InteropServices;

namespace LeafMachine
{

    public class MachineState
    {
        const int WIDTH = 40;
        const int HEIGHT = 25;
        const int NUM_COLORS = 16;

        public Color[] palette = new Color[NUM_COLORS+1];
        // 0 is bogus but will be referred to in images, meaning "background"/"transparent"
        // so 1..16 are the actual colors. for now, anyway.
        public int bgColor;  // index into the palette (1..16)

        // a 40x25 grid of characters, much like the C64
        public GraphicChar[,] chars = new GraphicChar[WIDTH,HEIGHT];
        public int[,] fgcolors = new int[WIDTH, HEIGHT];  // foreground colors => palette keys

        // Q: Do we really need to pass a GraphicsDeviceManager? what for?
        // we do use GraphicChar objects here, which hold actual images... or do we?
        // shouldn't we put something else as the character type for indexes [x,y]? a char or a string...
        public MachineState(GraphicsDeviceManager graphics)
        {
            SetC64Palette();  // for now, as a not-too-unreasonable default

            // clear the 40x25 grid
            for (int x = 0; x < 40; x++)
                for (int y = 0; y < HEIGHT; y++)
                    chars[x, y] = null;  // maybe EmptyChar or something?

            // set the foreground colors
            for (int x = 0; x < WIDTH; x++)
                for (int y = 0; y < HEIGHT; y++)
                    fgcolors[x, y] = 15;  // light blue, in C64 palette

            // set the background color
            bgColor = 7;  // blue, in C64 palette
        }

        protected void SetC64Palette()
        {
            // NOTE: these indexes are one up from the ones actually used by the C64
            palette[1] = new Color(0, 0, 0);  // black
            palette[2] = new Color(255, 255, 255);  // white
            palette[3] = new Color(136, 0, 0);  // red
            palette[4] = new Color(170, 255, 238);  // cyan
            palette[5] = new Color(204, 68, 204);  // purple
            palette[6] = new Color(0, 204, 85);  // green
            palette[7] = new Color(0, 0, 170);  // blue
            palette[8] = new Color(238, 238, 119);  // yellow
            palette[9] = new Color(221, 136, 85);  // orange
            palette[10] = new Color(102, 68, 0);  // brown
            palette[11] = new Color(255, 119, 119);  // pink
            palette[12] = new Color(51, 51, 51);  // dark gray
            palette[13] = new Color(119, 119, 119);  // med gray
            palette[14] = new Color(170, 255, 102);  // light green
            palette[15] = new Color(0, 136, 255);  // light blue
            palette[16] = new Color(187, 187, 187);  // light gray
        }
    }
}
