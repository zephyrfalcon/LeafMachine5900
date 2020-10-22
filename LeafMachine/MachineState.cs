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
        // 0 is bogus but will be referred to elsewhere, meaning "background"
        // so 1..16 are the actual colors. for now, anyway.
        public Color bgColor;

        // a 40x25 grid of characters, much like the C64
        public GraphicChar[,] chars = new GraphicChar[WIDTH,HEIGHT];
        public int[,] fgcolors = new int[WIDTH, HEIGHT];  // foreground colors => palette keys

        public MachineState(GraphicsDeviceManager graphics)
        {
            // clear the 40x25 grid
            for (int x = 0; x < 40; x++)
                for (int y = 0; y < HEIGHT; y++)
                    chars[x, y] = null;  // maybe EmptyChar or something?

            // TODO: set the background color
        }
    }
}
