6 value max-colors
4 value max-pegs

: (first-codeword) ( n -- cw )
    1 swap 1- 0 do 10 * 1+ loop ;

: (last-codeword) ( n -- cw )
    max-colors swap 1- 0 do 10 * max-colors + loop ;

max-pegs (first-codeword) value first-codeword
max-pegs (last-codeword) value last-codeword

: (next-codeword) ( cw,c -- cw' )
    if
        10 /mod
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

