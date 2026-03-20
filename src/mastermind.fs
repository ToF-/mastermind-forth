6 value max-colors
4 value max-pegs

: (first-codeword) ( n -- cw )
    1 swap 1- 0 do 10 * 1+ loop ;

: (last-codeword) ( n -- cw )
    max-colors swap 1- 0 do 10 * max-colors + loop ;

max-pegs (first-codeword) value first-codeword
max-pegs (last-codeword) value last-codeword

: next-codeword ( cw -- cw'|0 )
    10 /mod
    over max-colors = if
        nip 1+ 1
    else
        swap 1+
    then
    swap 10 * + ;

