
namespace LeafMachine
{

    public class Spinner
    {
        int _tixToChange;
        CharInfo[] _chars;
        int _x, _y;
        int _currentIndex = 0;

        public Spinner(int x, int y, CharInfo[] chars, int tixToChange ) {
            _tixToChange = tixToChange;
            _currentIndex = 0;  // maybe should be: LAST index in _chars?
            _chars = chars;
            _x = x;
            _y = y;
        }

        public CharInfo GetCharInfo(int x, int y, int tix)
        {
            if (tix % _tixToChange == 0) {
                _currentIndex++;
                if (_currentIndex >= _chars.Length)
                    _currentIndex = 0;
            }
            return _chars[_currentIndex];
        }
    }
}