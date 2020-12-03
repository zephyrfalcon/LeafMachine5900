using System.Collections.Generic;
using LeafMachine.CharSets;
using Microsoft.Xna.Framework;

namespace LeafMachine
{
    /* A GraphicCharSet holds a CharSet instance and will keep a dictionary of actual *images* (in Texture2D
       form), keyed by character/name.
    */
    public class GraphicCharSet
    {
        GraphicsDeviceManager _graphics;
        CharSet charset;
        Dictionary<string, GraphicChar> graphicChars;

        public GraphicCharSet(GraphicsDeviceManager graphics, CharSet aCharSet)
        {
            _graphics = graphics;
            charset = aCharSet;
            Init();
        }

        public GraphicChar Get(string name)
        {
            return graphicChars[name];
        }

        private void Init()
        {
            graphicChars = new Dictionary<string, GraphicChar> { };
            foreach (string name in charset.KnownChars()) {
                graphicChars[name] = new HiresChar(_graphics, charset.BitmapForChar(name));
            }
        }
    }
}