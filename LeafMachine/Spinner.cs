
namespace LeafMachine
{

    public class Spinner
    {
        int _tixToChange;
        int _currentTix = 0;
        CharInfo[] _chars;
        int _x, _y;

        public Spinner(int x, int y, CharInfo[] chars, int tixToChange ) {
            _tixToChange = tixToChange;
            _currentTix = 0;
            _chars = chars;
            _x = x;
            _y = y;
        }

        public CharInfo GetCharInfo(int x, int y, int tix)
        {
            throw new System.Exception("not implemented");
        }
    }
}