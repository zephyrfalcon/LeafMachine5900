using System.Collections.Generic;

namespace LeafMachine
{
    /* Holds multiple GraphicCharSet instances, keyed by name (a string).
    */
    public class GraphicCharSetManager
    {
        Dictionary<string, GraphicCharSet> graphicCharSets;

        public GraphicCharSetManager()
        {
            graphicCharSets = new Dictionary<string, GraphicCharSet> { };
        }

        public void Add(string name, GraphicCharSet aGraphicCharSet)
        {
            graphicCharSets.Add(name, aGraphicCharSet);
        }

        /* Get a character (as a GraphicChar) from a specific charset. */
        public GraphicChar Get(string setname, string charname)
        {
            GraphicCharSet gcs = graphicCharSets[setname];
            return gcs.Get(charname);
        }
    }
}