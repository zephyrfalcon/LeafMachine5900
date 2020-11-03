using System;

namespace LeafMachine
{
    public static class LeafMachine
    {
        const string VERSION = "0.0.0/M1";

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
