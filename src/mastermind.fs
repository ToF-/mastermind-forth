require random.fs


6 constant max-colors
4 constant max-pegs

max-pegs 10 * constant victory

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


999999 constant max-score

create scores 100 cells allot

: init-scores!
    scores 100 cells erase ;

: score++! ( r -- )
    cells scores +
    dup @ 1+ swap ! ;

: score-max ( -- sc )
    0
    100 0 do
        i cells scores + @ max
    loop ;

variable score

: .set ( addr -- )
    dup first-in-set
    begin
        ?dup while
        dup .
        over next-in-set
    repeat
    drop ;
    
: match-result-scores! ( cw,addr -- )
    dup first-in-set
    begin
        ?dup while                 \ cw,addr,ca
        swap -rot 2dup             \ addr,cw,ca,cw,ca
        match-result score++!      \ addr,cw,ca
        rot tuck next-in-set       \ cw,addr,cb
    repeat
    2drop ;

: max-match-result-score ( cw,addr -- sc )
    init-scores!
    match-result-scores!
    score-max ;

variable min-score
variable min-codeword

999999 constant max-score

: minmax-match-result-score ( addr -- cw )
    max-score min-score !
    first-codeword
    begin
        ?dup while
        swap 2dup max-match-result-score   \ cw,addr,sc
        >r 2dup member-or-zero?
        if 0 else 1 then r>
        2* + min-score @ over > if         \ cw,addr,sc
            min-score !
            over min-codeword !
        else
            drop
        then
        swap next-codeword
    repeat
    drop min-codeword @ ;

6214 value secret

: guess-move ( cw,r,addr -- cw',r' )
   dup 2swap rot narrow
   minmax-match-result-score
   dup secret match-result ;

variable moves

codeword-set solution

: random-codeword ( -- cw )
    0 max-pegs 0 do
        10 * max-colors random 1+ +
    loop ;

: random-secret
    random-codeword to secret ;

: valid-codeword? ( cw -- f )
    true swap
    max-pegs 0 do
        10 /mod
        swap 1 max-colors within 
        rot and swap
    loop drop ;

: set-length ( addr -- n )
    0 swap
    dup first-in-set
    begin
        ?dup while
        rot 1+ -rot
        over next-in-set
    repeat
    drop ;


        
: guess
    cr
    moves off
    solution set-init!
    1122
    dup secret match-result
    begin
        1 moves +!
        moves @ 2 .r space
        2dup swap 6 .r space 2 .r 9 emit solution set-length . cr
        dup victory <> while
        solution guess-move
    repeat
    2drop ;

        


