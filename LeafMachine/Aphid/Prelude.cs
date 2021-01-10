namespace LeafMachine.Aphid
{
    public static class Prelude
    {
        public static string code = @"

; 2dup ( x y -- x y x y )
{ over over } :2dup defword

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

; setxy* ( x y fgcolor charset charname -- )
{ current-charset pushr
  swap                      ; x y fgcolor charname charset
  set-current-charset       ; x y fgcolor charname
  setxy
  popr set-current-charset
} :setxy* defword

; writexy* ( x y fgcolor charset charname -- )
{ current-charset pushr
  swap                      ; x y fgcolor charname charset
  set-current-charset       ; x y fgcolor charname
  writexy
  popr set-current-charset
} :writexy* defword

; even? ( x -- bool )
{ 2 rem 0 int= } :even? defword

; even? ( x -- bool )
{ even? not } :odd? defword

; sum ( list-of-numbers -- sum )
{ { + } 0 reduce } :sum defword

";
    }
}

/* Ideas/thoughts:

   - We can write a word `++` (or maybe, `inc`) that takes a variable name (so, a symbol)
     and adds 1 to it. Similar for `dec`. [done] 
     Also `inc-by` and `dec-by`. [TBD]
   - `pairwise`, based on `for-by` but returns two items on the stack for the block, instead of a list.
     `list block pairwise`
     so `[ 1 2 3 4 5 6 ] { + } pairwise` => `[ 3 7 11 ]`
*/
