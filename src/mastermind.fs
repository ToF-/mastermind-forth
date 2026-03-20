6 value max-colors
4 value max-pegs

: (first-codeword) ( n -- cw )
    1 swap 1- 0 do 10 * 1+ loop ;

: (last-codeword) ( n -- cw )
    max-colors swap 1- 0 do 10 * max-colors + loop ;

max-pegs (first-codeword) value first-codeword
max-pegs (last-codeword) value last-codeword

: unit/tenth ( cw -- c,cw' )
    10 /mod ;

: (next-codeword) ( cw,c -- cw' )
    if
        unit/tenth
        swap dup max-colors < if
            1+
        else
            drop 1 recurse 1
        then
        swap 10 * +
    then ;

: next-codeword ( cw -- cw'|0 )
    dup last-codeword < if
        1 (next-codeword)
    else
        drop 0
    then ;

create factors
    1 , 10 , 100 , 1000 , 10000 , 100000 , 1000000 , 10000000 , 100000000 ,

: factor ( n -- 10^n )
    1- cells factors + @ ;

: (color-tally) ( acc,cw -- colors )
    ?dup if
        unit/tenth
        swap factor
        rot + swap
        recurse
    then ;

: color-tally ( cw -- colors )
    0 swap (color-tally) ;

: hits ( cw,cw' -- n )
    0 -rot color-tally
    swap color-tally
    max-colors 0 do
        unit/tenth
        swap rot unit/tenth
        -rot min
        -rot 2swap + -rot
    loop
    2drop ;

: matches ( cw,cw' -- n )
    0 -rot
    max-pegs 0 do
        unit/tenth
        swap rot unit/tenth
        -rot = if
            rot 1+ -rot
        then
    loop
    2drop ;

: match ( cw,cw' -- r )
    2dup matches -rot
    hits over -
    swap 10 * + ;
    
