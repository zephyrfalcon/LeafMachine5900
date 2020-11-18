namespace LeafMachine.Aphid
{
    public static class Prelude
    {
        public static string code = @"

; when ( cond block -- ? )
{ { } if } :when defword

";
    }
}

/* Ideas/thoughts:

   - We can write a word `++` (or maybe, `inc`) that takes a variable name (so, a symbol)
     and adds 1 to it. Similar for `dec`. Also `inc-by` and `dec-by`.

   - `nop` could be moved into the prelude (and would then need to be removed from m3-colors.aphid).
     Not spectacular, but a no-op updater (or a no-op word in general) is useful.

   - Since m3-colors.aphid has a word `second-passed?`, maybe we can add a word `tix-passed?`
     to the prelude that checks if N tix have passed?
     ` 4 tix-passed? { ... } when `
*/
