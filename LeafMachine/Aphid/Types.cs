namespace LeafMachine.Aphid.Types
{
    public class AphidType
    {

    }

    public class AphidInteger : AphidType
    {
        private int value = 0;
        public AphidInteger() { }
        public AphidInteger(int x) { value = x; }
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
    }
}