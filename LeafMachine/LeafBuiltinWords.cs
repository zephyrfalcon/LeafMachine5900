using System.Collections.Generic;
using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;

namespace LeafMachine
{
    public class LeafBuiltinWords
    {
        public void WriteXY(AphidInterpreter aip, MachineState state)
        {
            // do stuff...
        }

        /* built-in words */

        public Dictionary<string, DelAphidLeafBuiltinWord> GetBuiltinWords()
        {
            Dictionary<string, DelAphidLeafBuiltinWord> bw = new Dictionary<string, DelAphidLeafBuiltinWord> {
                { "writexy", WriteXY },
            };
            return bw;
        }

    }
}