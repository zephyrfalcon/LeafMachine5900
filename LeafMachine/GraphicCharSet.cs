using System.Collections.Generic;
using LeafMachine.CharSets;
using Microsoft.Xna.Framework;

namespace LeafMachine
{
    /* A GraphicCharSet holds a CharSet instance and will keep a dictionary of actual *images* (in Texture2D
       form), keyed by character/name.

       TODO: It should be possible to dynamically add characters, to both the CharSet and the GraphicCharSet.
       Images should be generated automagically.
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

        public CharSet GetCharSet()
        {
            return charset;
        }

        // XXX consider making public in case we want to regenerate images for the whole CharSet
        private void Init()
        {
            graphicChars = new Dictionary<string, GraphicChar> { };
            foreach (string name in charset.KnownChars()) {
                graphicChars[name] = new HiresChar(_graphics, charset.BitmapForChar(name));
            }
        }

        // If a character was added to the CharSet, call AddGraphicChar to make sure we have
        // the appropriate GraphicChar for it. Can also be used to refresh existing chars
        // if necessary.
        public void AddGraphicChar(string charname)
        {
            graphicChars[charname] = new HiresChar(_graphics, charset.BitmapForChar(charname));
        }
    }
}