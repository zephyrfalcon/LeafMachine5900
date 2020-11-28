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

{ random 1 + } :random1 defword

{ ] dict } :]! defword
";
    }
}

/* Ideas/thoughts:

   - We can write a word `++` (or maybe, `inc`) that takes a variable name (so, a symbol)
     and adds 1 to it. Similar for `dec`. [done] 
     Also `inc-by` and `dec-by`.
*/
