using LeafMachine;
using System.IO;

namespace LeafMachine
{
    public class Tools {
        public string ExecutablePath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
    }
}