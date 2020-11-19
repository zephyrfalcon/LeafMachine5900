namespace LeafMachine.Aphid
{
    public static class Prelude
    {
        public static string code = @"

; when ( cond block -- ? )
{ { } if } :when defword

; inc ( variable-name -- )
{ dup                ; name name
  getvar             ; name value
  1 +                ; name value+1
  swap               ; value+1 name
  setvar
} :inc defword

; dec ( variable-name -- )
{ dup getvar -1 + swap setvar } :dec defword

{ } :nop defword

; tix-passed? ( n -- [true if tix%n == 0, false otherwise] )
{ tix              ; n tix
  swap             ; tix n
  rem              ; tix%n
  0 int=           ; bool
} :tix-passed? defword

{ 60 tix-passed? } :second-passed? defword

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
