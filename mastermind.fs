
6 CONSTANT MAXCOLOR
4 CONSTANT N

VARIABLE PATTERN
VARIABLE TEST

: PEG? ( n -- f )
    1 MAXCOLOR 1+ WITHIN ;

: PATTERN? ( n -- f )
    TRUE SWAP
    N 0 DO
        10 /MOD
        SWAP PEG? ROT AND SWAP
    LOOP DROP ;

CREATE PATTERNCOLORS MAXCOLOR 1+ ALLOT
CREATE TESTCOLORS MAXCOLOR 1+ ALLOT


: TALLY-COLORS ( pattern,addr )
    DUP MAXCOLOR 1+  ERASE SWAP
    N 0 DO
        OVER >R
        10 /MOD
        SWAP R> +
        DUP C@ 1+ SWAP C!
    LOOP 2DROP ;

: HITS ( -- n )
    0 MAXCOLOR 1+ 1 DO
        PATTERNCOLORS I + C@
        TESTCOLORS I + C@ MIN
        +
    LOOP ;

: MISSES ( -- n )
    0 MAXCOLOR 1+ 1 DO
        PATTERNCOLORS I + C@
        TESTCOLORS I + C@
        - 0 MAX
        +
    LOOP ;

: BLACK-HITS ( -- n )
    0 PATTERN @ TEST @ 
    N 0 DO
        10 /MOD SWAP ROT
        10 /MOD SWAP ROT
        = IF
            ROT 1+ -ROT
        THEN
    LOOP 2DROP ;

: WHITE-HITS ( -- n )
    HITS BLACK-HITS - ;

: INIT-PATTERN ( n -- )
    DUP PATTERN !
    PATTERNCOLORS TALLY-COLORS ;

: INIT-TEST ( n -- )
    DUP TEST !
    TESTCOLORS TALLY-COLORS ;

: SCORE ( p,t -- b,w )
    INIT-TEST
    INIT-PATTERN
    BLACK-HITS
    WHITE-HITS ;
    
: .SCORE ( p,t -- )
    SCORE SWAP . . CR ;

1111 CONSTANT FIRST-CODEWORD

: NEXT-CODEWORD-REC ( n -- n' )
    10 /MOD SWAP DUP 6 < IF
        1+
    ELSE
        DROP RECURSE 1
    THEN
    SWAP 10 * + ;

: NEXT-CODEWORD ( n -- n' )
    DUP 6666 = IF DROP -1 ELSE NEXT-CODEWORD-REC THEN ;


VARIABLE MAX-POSSIBLE

: .POSSIBLE-CODEWORDS ( p b w -- )
    0 MAX-POSSIBLE !
    ROT FIRST-CODEWORD              \ b w p c
    BEGIN
        DUP -1 <> WHILE 
        2DUP SCORE                  \ b w p c b' w'
        2>R 2OVER 2R> D= IF
            DUP .
            1 MAX-POSSIBLE +!
            MAX-POSSIBLE @ 16 MOD 0= IF CR THEN
        THEN
        NEXT-CODEWORD
    REPEAT 2DROP 2DROP 
    CR MAX-POSSIBLE ? ;


