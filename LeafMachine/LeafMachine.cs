using System;

namespace LeafMachine
{
    public static class LeafMachine
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Machine())
                game.Run();
        }
    }
}
