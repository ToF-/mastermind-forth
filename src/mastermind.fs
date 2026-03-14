6 constant max-colors
4 constant max-pegs

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
    


