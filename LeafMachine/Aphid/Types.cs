using System.Security.Cryptography.X509Certificates;

namespace LeafMachine.Aphid.Types
{
    public class AphidType
    {
        // should this be abstract?
    }

    public class AphidInteger : AphidType
    {
        private int value = 0;
        public AphidInteger() { }
        public AphidInteger(int x) { value = x; }
        public override string ToString() { return this.value.ToString(); }
    }

    public class AphidBuiltinWord : AphidType
    {
        string name;
        DelAphidBuiltinWord builtinWord;
        // a built-in word must always have a name
        public AphidBuiltinWord(string aname, DelAphidBuiltinWord abuiltinWord)
        {
            name = aname;
            builtinWord = abuiltinWord;
        }

        public override string ToString()
        {
            return "<to be implemented>";
            // TODO: some string interpolation here
        }
    }
}