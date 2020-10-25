using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;

namespace LeafMachine
{
    public delegate void DelAphidLeafBuiltinWord(AphidInterpreter aip, MachineState state);

    public class AphidLeafBuiltinWord : AphidWord
    {
        MachineState state;
        DelAphidLeafBuiltinWord builtinWord;

        public AphidLeafBuiltinWord(MachineState astate, string aname, DelAphidLeafBuiltinWord aBuiltinWord)
        {
            this.state = astate;
            this.name = aname;
            this.builtinWord = aBuiltinWord;
        }

        public override string ToString()
        {
            return $"<{name}>";
        }

        public override void Run(AphidInterpreter aip)
        {
            builtinWord(aip, state);
        }

    }
}
