using System;

namespace LeafMachine
{
    public static class LeafMachine
    {
        [STAThread]
        static void Main(string[] args)
        {
            string mainfile = "";
            if (args.Length > 0) {
                mainfile = System.IO.Path.GetFullPath(args[0]);
            }

            using (var game = new Machine(mainfile))
                game.Run();
        }
    }
}
