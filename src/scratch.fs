require random.fs

6 value max-colors
4 value max-pegs
4614 value secret

: all-match ( -- r )
    max-pegs 10 * ;

: (first-codeword) ( n -- cw )
    1 swap 1- 0 do 10 * 1+ loop ;

: (last-codeword) ( n -- cw )
    max-colors swap 1- 0 do 10 * max-colors + loop ;

max-pegs (first-codeword) value first-codeword
max-pegs (last-codeword) value last-codeword

: unit/tenth ( cw -- c,cw' )
    10 /mod ;

: random-codeword ( -- cw )
    0 max-pegs 0 do
        10 * max-colors random 1+ +
    loop ;

: valid-codeword? ( cw -- f )
    true swap
    max-pegs 0 do
        unit/tenth
        swap 1 max-colors within 
        rot and swap
    loop drop ;

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

variable max-guesses
create guess-codewords 10 cells allot
create guess-results 10 cells allot

: add-guess ( cw,r -- )
    max-guesses @ cells dup 
    rot swap guess-results + !
    guess-codewords + !
    1 max-guesses +! ;

: guess#codeword ( i -- cw )
    cells guess-codewords + @ ;

: result# ( i -- r )
    cells guess-results + @ ;

: guess#-match? ( cw,i -- f )
    dup guess#codeword swap result#
    -rot match = ;

256 constant interval-capacity

: intervals ( < name > -- )
    create 0 , interval-capacity cells allot ;

: max-intervals ( ints -- n )
    @ ;

: add-interval ( start,end,ints -- )
    dup max-intervals dup assert( interval-capacity < )
    1+ cells over +           \ start,end,ints,addr
    2swap 16 lshift or        \ ints,addr,int
    swap ! 1 swap +! ;

: current-interval ( ints -- start,end )
    dup max-intervals ?dup if
        cells + @ dup 65535 and swap 16 rshift
    else
        drop 0 0
    then ;

: initial-interval ( ints -- )
    dup off
    first-codeword last-codeword rot add-interval ;

intervals intervals-a
intervals intervals-b

intervals-a value source-intervals
intervals-b value target-intervals

: switch-intervals
    target-intervals
    source-intervals to target-intervals
    to source-intervals ;

variable in-solution
variable interval-stop
variable interval#

: keep-codeword ( cw )
    in-solution @ if
        46 emit
    else
        in-solution on
        1 interval# +!
        interval# ? [char] : emit  dup .
    then interval-stop ! ;

: drop-codeword
    in-solution @ if
        interval-stop @ . cr
    then drop
    in-solution off ;

: keep-compatible ( cw,r -- )
    interval# off
    in-solution off
    first-codeword
    begin
        ?dup while
        >r 2dup swap r@ match = if
            r@ keep-codeword
        else
            r@ drop-codeword
        then
        r> next-codeword
    repeat 2drop ;

: in-solution? ( cw -- f )
    ?dup if 
        true swap
        max-guesses @ 0 ?do
            dup i guess#-match?
            rot and swap
            over 0= if leave then
        loop 
        drop
    else
        true
    then ;

: (next-candidate) ( cw -- cw' )
    begin
        dup in-solution? 0= while
        next-codeword
    repeat ;
    
: next-candidate ( cw -- cw'|0 )
    dup last-codeword < if 
        next-codeword
        (next-candidate)
    else
        drop 0
    then ;

: first-candidate ( -- cw )
    first-codeword
    dup in-solution? 0= if
        (next-candidate)
    then ;

: .candidates
    first-candidate
    begin
        dup while
        dup .
        next-candidate
    repeat drop ;

create result-scores 100 cells allot

: init-result-scores
    result-scores 100 cells erase ;

100 constant max-results

: result-score# ( i -- n )
    cells result-scores + @ ;

: result-score#++ ( n -- )
    1 swap cells result-scores + +! ;

: max-result-score ( -- n )
    0 max-results 0 do
        i result-score# max
    loop ;

: max-match-result-score ( cw -- n )
    init-result-scores
    first-candidate
    begin
        ?dup while
        2dup match result-score#++
        next-candidate
    repeat drop
    max-result-score ;

1000000 constant infinity

: min-max-match-result-score ( -- cw )
    0 infinity
    first-codeword
    begin
        ?dup while                \ cw,min,cw'
        dup max-match-result-score   \ cw,min,cw',n
        2* over in-solution?      \ cw,min,cw',n,f
        0= if 1+ then             \ cw,min,cw',n
        2swap rot                 \ cw',cw,min,n
        2dup > if
            -rot 2drop over       \ cw',n,cw'
        else
            drop rot              \ cw,min,cw'
        then
        next-codeword
    repeat
    drop ;

: last-guess-result ( -- r )
    max-guesses @ 1- result# ;

: last-guess-codeword ( -- cw )
    max-guesses @ 1- guess#codeword ;

: guess ( -- n )
    max-guesses off
    1122 dup secret match add-guess
    begin
        max-guesses @ . last-guess-codeword . last-guess-result . cr
        last-guess-result all-match <> while
        min-max-match-result-score
        secret over match add-guess
    repeat
    max-guesses @ ;

: .guesses
    max-guesses @ 0 do
        i 1+ . [char] ) emit space
        i guess#codeword .
        i result# . cr
    loop ;

