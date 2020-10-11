namespace LeafMachine.Aphid
{
    public class Tokenizer
    {
        // very basic version for now; simply splits the string on whitespace; does not take
        // into account spaces inside string literals, comments, etc.
        public static string[] Tokenize(string data)
        {
            char[] delimiters = { ' ', '\n', '\t', '\r' };
            return data.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}