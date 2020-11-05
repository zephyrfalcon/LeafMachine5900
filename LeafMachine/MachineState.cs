using Microsoft.Xna.Framework;
using LeafMachine;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using LeafMachine.CharSets;

namespace LeafMachine
{

    public class MachineState
    {
        public const int WIDTH = 40;
        public const int HEIGHT = 25;
        public const int NUM_COLORS = 16;

        GraphicsDeviceManager _graphics;

        public Color[] palette = new Color[NUM_COLORS+1];
        // 0 is bogus but will be referred to in images, meaning "background"/"transparent"
        // so 1..16 are the actual colors. for now, anyway.
        public int bgColor;  // index into the palette (1..16)

        // a 40x25 grid of characters, much like the C64
        public string[,] chars = new string[WIDTH,HEIGHT];  // letters or names like "a", "heart", etc
        public int[,] fgcolors = new int[WIDTH, HEIGHT];  // foreground colors => palette keys

        public Dictionary<string, GraphicChar> graphicChars;

        // Q: Do we really need to pass a GraphicsDeviceManager? what for?
        // we do use GraphicChar objects here, which hold actual images... or do we?
        // shouldn't we put something else as the character type for indexes [x,y]? a char or a string...
        public MachineState(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;

            // load some character bitmaps as images, using the C64 charset for now
            // we'll get back to this later
            InitGraphicChars();

            SetC64Palette();  // for now, as a not-too-unreasonable default

            // clear the 40x25 grid
            for (int x = 0; x < 40; x++)
                for (int y = 0; y < HEIGHT; y++)
                    chars[x, y] = " ";

            // set the foreground colors
            for (int x = 0; x < WIDTH; x++)
                for (int y = 0; y < HEIGHT; y++)
                    fgcolors[x, y] = 15;  // light blue, in C64 palette

            // set the background color
            bgColor = 7;  // blue, in C64 palette
        }

        public void SetChar(int x, int y, string name)
        {
            chars[x, y] = name;
        }

        public void SetColor(int x, int y, int c)
        {
            fgcolors[x, y] = c;
        }

        protected void InitGraphicChars()
        {
            graphicChars = new Dictionary<string, GraphicChar> { };
            C64CharSet cs = new C64CharSet();
            foreach (string name in cs.KnownChars()) {
                graphicChars[name] = new HiresChar(_graphics, cs.BitmapForChar(name));
            }
        }

        protected void SetC64Palette()
        {
            palette[1] = new Color(0, 0, 0);  // black
            palette[2] = new Color(255, 255, 255);  // white
            palette[3] = new Color(157, 67, 57);  // red
            palette[4] = new Color(110, 200, 206);  // cyan
            palette[5] = new Color(163, 55, 183);  // purple
            palette[6] = new Color(80, 185, 75);  // green
            palette[7] = new Color(86, 29, 172);  // blue
            palette[8] = new Color(206, 229, 120);  // yellow
            palette[9] = new Color(160, 104, 33);  // orange
            palette[10] = new Color(105, 84, 0);  // brown
            palette[11] = new Color(204, 124, 116);  // pink
            palette[12] = new Color(96, 96, 96);  // dark gray
            palette[13] = new Color(138, 138, 138);  // med gray
            palette[14] = new Color(154, 244, 145);  // light green
            palette[15] = new Color(145, 108, 224);  // light blue
            palette[16] = new Color(179, 179, 179);  // light gray
        }
    }
}
