require random.fs

6 constant max-colors
4 constant max-pegs

: (max-codewords) ( -- n )
    1 max-pegs 0 do 6 * loop ;

 (max-codewords) constant max-codewords

max-pegs 10 * constant victory

: codeword-struct ( <name> -- )
    create max-pegs max-colors + allot ;

: colors> ( addr -- addr' )
    max-pegs + ;

: (nth-codeword!) ( n,addr -- )
    dup max-pegs + swap
    do
        max-colors /mod
        swap i c!
    loop drop ;

: (colors!) ( addr -- )
    max-pegs +
    dup max-colors erase
    dup dup max-pegs -
    do
        i c@ over +
        dup c@ 1+ swap c!
    loop drop ;

: nth-codeword! ( n,addr -- )
    tuck (nth-codeword!) (colors!) ;

: (codeword!) ( n,addr -- )
    dup max-pegs + swap do
        10 /mod swap 1- i c!
    loop drop ;

: codeword! ( n,addr -- )
    tuck (codeword!) (colors!) ;
        
: codeword ( addr -- n )
    0 1 rot dup max-pegs + swap
    do
        dup i c@ 1+ *
        rot + swap
        10 *
    loop drop ;

: matches ( addr,addr -- n )
    0 -rot
    max-pegs 0 do
        2dup i + c@ swap i + c@ = if
            rot 1+ -rot
        then
    loop 2drop ;

: hits ( addr,addr -- n )
    0 -rot colors>
    swap colors>
    max-colors 0 do
        2dup i + c@ swap i + c@
        min >r rot r> + -rot
    loop 2drop ;

: match-result ( addr,addr -- r )
    2dup hits
    -rot matches
    dup 10 * -rot - + ;

: codeword-set ( <name> addr -- )
    create , 0
    here max-codewords 8 /
    dup allot erase ;

: (set-codeword!) ( addr -- )
    dup cell + @ swap @ nth-codeword! ;

: first-codeword!? ( addr -- f )
    0 over cell + ! (set-codeword!) true ;

: next-codeword!? ( addr -- f )
    dup cell + @ max-codewords 1- < if
        1 over cell + +! (set-codeword!) true
    else
        drop false
    then ;
    
false [IF]

: pegs-first! ( addr -- )
    dup max-pegs + swap do
        1 i c!
    loop ;

: pegs-next! ( addr -- f )
    1 swap
    dup max-pegs + swap do
        i c@ +
        dup max-colors = if
            drop 1 0
        else
            0 swap
        then
        i c!
    loop
    0= ;

: codeword ( addr -- cw )
    0 
    max-pegs 0 do
        10 * over
        max-pegs i - + 1-
        c@ 1+ +
    loop nip ;

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

pegs this
pegs that
pegs other

: narrow ( cw,r,addr -- )
    rot this decompose
    swap >r
    first-codeword
    begin
        ?dup while               \ addr,ca
        dup that decompose
        this that match-result \ addr,ca,r'
        r@ <> if                 \ addr,ca
            2dup swap remove
        then
        next-codeword
    repeat
    drop r> drop ;


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
    swap this decompose
    dup first-in-set
    begin
        ?dup while
        dup that decompose
        this that match-result score++!
        over next-in-set
    repeat
    drop ;

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
   dup this decompose
   this other match-result ;

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
    1122 dup this decompose
    secret other decompose
    moves off
    solution set-init!
    cr
    this other match-result
    begin
        1 moves +!
        moves @ 2 .r space
        2dup swap 6 .r space 2 .r cr
        dup victory <> while
        solution guess-move
    repeat
    2drop ;
[THEN]

