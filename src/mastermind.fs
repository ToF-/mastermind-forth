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

last-codeword 8 / 1+ constant set-size

: codeword-set ( <name> ) 
    create set-size allot ;

: set-init! ( addr -- )
    set-size 255 fill ;

: member-or-zero? ( cw,addr -- f )
    swap 8 /mod rot +
    c@ 1 rot lshift and ;

: first-in-set ( addr -- cw )
    first-codeword
    begin
        2dup swap member-or-zero? 0= while
        next-codeword
    repeat
    nip ;

: next-in-set ( cw,addr -- cw )
    swap next-codeword 
    begin
        2dup swap member-or-zero? 0= while
        next-codeword
    repeat
    nip ;

: remove ( cw,addr -- )
    swap 8 /mod rot +     \ bit,addr'
    dup c@
    rot 1 swap lshift 255 xor
    and swap c! ;

: narrow ( cw,r,addr -- )
    first-codeword
    begin
        ?dup while               \ cw,r,addr,ca
        2over -rot               \ cw,r,addr,r,ca,cw
        over match-result rot    \ cw,r,addr,ca,r',r
        <> if
            2dup swap remove
        then
        next-codeword
    repeat
    drop 2drop ;
