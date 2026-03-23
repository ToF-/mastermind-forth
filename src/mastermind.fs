require random.fs


6 value max-colors
4 value max-pegs

create guess-pegs max-pegs allot
create secret-pegs max-pegs allot
create guess-colors max-colors 1+ allot
create secret-colors max-colors 1+ allot

: peg-at ( i,addr -- p )
    + c@ ;

: codeword>pegs! ( cw,addr -- )
    max-pegs 0 do                    \ cw,addr
        dup i +                      \ cw,addr,addr'
        rot 10 /mod                  \ addr,addr',p',cw'
        swap rot c! swap             \ cw',addr'
    loop 2drop ;

: colors! ( pegs,colors -- )
    dup max-colors 1+ erase
    max-pegs 0 do
        2dup swap i peg-at +
        dup c@ 1+ swap c!
    loop 2drop ;

: matches ( addr1,addr2 -- n )
    0 -rot max-pegs 0 do
        2dup i swap peg-at
        i rot peg-at
        = if rot 1+ -rot then
    loop 2drop ;

: hits ( -- n )
    secret-pegs secret-colors colors!
    guess-pegs guess-colors colors!
    0 max-colors 1+ 1 do
        secret-colors i + c@
        guess-colors i + c@
        min +
    loop ;

: match-result ( cw,cw' -- r )
    secret-pegs codeword>pegs!
    guess-pegs codeword>pegs!
    guess-pegs secret-pegs matches
    hits over - swap 10 * + ;
