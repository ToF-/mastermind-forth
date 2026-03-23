require random.fs


6 constant max-colors
4 constant max-pegs

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

: (first-codeword) ( -- cw )
    0 max-pegs 0 do
        10 * 1 +
    loop ;

(first-codeword) value first-codeword

: (last-codeword) ( -- cw )
    0 max-pegs 0 do
        10 * max-colors +
    loop ;

(last-codeword) value last-codeword

: (next-codeword) ( cw -- cw' )
    1+ 10 /mod
    over max-colors > if
        recurse nip 1 swap
    then
    10 * + ;

: next-codeword ( cw -- cw|0 )
    dup last-codeword < if
        (next-codeword) 
    else
        drop 0
    then ;

1024 constant max-intervals

: {} ( <name> ) 
    create 0 , max-intervals cells allot ;

: {}init! ( addr -- )
    0 !

: }interval-max ( addr -- n )
    w@ ;

: }intervals ( addr - addr )
    cell + ;

: }current-interval ( addr -- addr )
    dup 2 + w@ 8 * swap intervals + ;

:interval-end ( addr -- cw )
    4 + w@ ;

: }current ( addr -- cw )
    4 + w@ ;

: }current! ( cw,addr -- )
    4 + w! ;

: (}next) ( addr -- cw|0 )
    r>
    r@ }current-interval interval-end
    r@ }current dup rot < if
        next-codeword
        dup r@ }current!
    else

: }next ( addr -- cw|0 )
    dup }current last-codeword < if
        (}next)
    else
        drop 0
    then ;

    
    
