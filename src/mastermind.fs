6 constant max-colors
4 constant max-pegs


: c++! ( addr -- )
    dup c@ 1+ swap c! ;

: ++! ( addr -- )
    1 swap +! ;

: ^ ( x,y -- x^y )
    1 swap 0 do over * loop nip ;

max-colors max-pegs ^ constant max-codewords

: number>pegs ( n -- p1,p2,p3,p4 )
    max-pegs 1- 0 do
        max-colors /mod
    loop ;

: pegs>codeword ( p1,p2,p3,p4 -- cw )
    0
    max-pegs 0 do
        10 * swap 1+ +
    loop ;

: number>codeword ( n -- cw )
    number>pegs pegs>codeword ;

: codeword>digits ( n -- d1,d2,d3,d4 )
    max-pegs 1- 0 do
        10 /mod
    loop ;

: digits>number ( d1,d2,d3,d4 -- n )
    0 max-pegs 0 do
        max-colors * swap 1- +
    loop ;

: codeword>number ( cw -- n )
    codeword>digits digits>number ;

: offset,bits ( n -- offest,bits )
    8 /mod 1 rot lshift ;

: bitset-member? ( n,bitset -- f )
    swap offset,bits
    -rot + c@ and ;

: bitset-insert ( n,bitset -- )
    swap offset,bits
    -rot + dup c@
    rot or
    swap c! ;

: bitset-remove ( n,bitset -- )
    swap offset,bits 255 xor
    -rot + dup c@
    rot and
    swap c! ;

: codewords ( <name> )
    create -1 , max-codewords 8 / allot ;

: codewords-start! ( cws -- )
    -1 swap ! ;

: all-codewords! ( cws -- )
    dup cell+ max-codewords 8 / 255 fill
    codewords-start! ;

: empty-codewords! ( cws -- )
    dup cell+ max-codewords 8 / erase
    codewords-start! ;

: (codeword-number) ( cw,cws -- n,bitset )
    cell+ swap codeword>number swap ;

: codeword-member? ( cw,cws -- f )
    (codeword-number) bitset-member? ;

: codeword-remove ( cw,cws -- )
    (codeword-number) bitset-remove ;

: codeword-insert ( cw,cws -- )
    (codeword-number) bitset-insert ;

: (next-codeword) ( cws -- )
    begin
        1 over +!
        dup @ dup max-codewords < -rot
        over cell+ bitset-member? 0=
        rot and while 
    repeat drop ;

: next-codeword ( cws -- cw|f )
    dup (next-codeword)
    @ dup max-codewords < if
        number>codeword
    else
        drop 0
    then ;

: matches ( cw1,cw2 -- n )
    0 -rot
    max-pegs 0 do
        10 /mod rot 10 /mod rot
        2swap = if
            rot 1+ -rot
        then
    loop 2drop ;

create color-count max-colors 2* allot

: pegs ( cw -- c1,c2,… )
    max-pegs 1- 0 do
        10 /mod
    loop ;

: count-colors ( c1,c2,…,addr -- )
    max-pegs 0 do
        swap 1- over + c++!
    loop drop ;

: hits ( cw1,cw2 -- n )
    color-count max-colors 2* erase
    pegs color-count count-colors
    pegs color-count max-colors + count-colors
    0 max-colors 0 do
        color-count i + c@
        color-count max-colors + i + c@
        min +
    loop ;

: misses ( cw1,cw2 -- n )
    2dup hits -rot matches - ;

: match  ( cw1, cw2 -- result )
    2dup matches 10 *
    -rot misses + ;

55 constant max-results \ max of results with 9 colors

create result-scores max-results cells allot

: init-results
    result-scores max-results cells erase ;

codewords all-codewords

: max-match-result ( cw,cws -- n )
    init-results
    dup codewords-start!
    begin
        2dup next-codeword ?dup while
        match cells result-scores + ++!
    repeat 2drop
    0 max-results 0 do
        result-scores i cells + @ max
    loop ;

variable min-max-result
variable min-max-codeword

: min-max-match-result ( cws -- cw )
    all-codewords all-codewords!
    1000000 min-max-result !
    begin
        all-codewords next-codeword ?dup while      \ cws,cw
        swap 2dup max-match-result                  \ cw,cws,r
        -rot 2dup swap codeword-member? if          \ r,cw,cws,cws
            rot 2*
        else
            rot 2* 1+
        then                                        \ cw,cws,r'
        dup min-max-result @ < if
            min-max-result !
            min-max-codeword !
        then                                        \ cw,cws
        nip
    repeat 
    min-max-codeword @ ;



