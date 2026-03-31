require ../src/mastermind.fs
require ffl/tst.fs

page

\ some values
max-colors 6 ?s
max-pegs   4 ?s
victory    40 ?s

\ codeword index to value

0 nth-codeword-value 1111 ?s
1 nth-codeword-value 1112 ?s
36 nth-codeword-value 1211 ?s
max-codewords 1- nth-codeword-value 6666 ?s

1111 codeword-number 0 ?s
1235 codeword-number 52 ?s
5431 codeword-number 984 ?s
6666 codeword-number 1295 ?s
bye

\ codeword structure 

codeword foo

0 foo codeword-index!
foo codeword-index 0 ?s
foo codeword-value 1111 ?s
false [IF]

codeword-struct foo
0 foo nth-codeword!

foo w@ hex 0 ?s decimal
foo 0 + c@ 0 ?s foo 1 + c@ 0 ?s foo 2 + c@ 0 ?s foo 3 + c@ 0 ?s
foo colors> c@ 4 ?s
1295 foo nth-codeword!
foo w@ hex 0505 ?s decimal foo 2 + w@ hex 0505 ?s decimal
foo colors> 5 + c@ 4 ?s

0    foo nth-codeword!  foo codeword 1111 ?s
1    foo nth-codeword!  foo codeword 1112 ?s
36   foo nth-codeword!  foo codeword 1211 ?s
51   foo nth-codeword!  foo codeword 1234 ?s
foo w@ hex 0203 ?s decimal foo 2 + w@ hex 0001 ?s decimal
1295 foo nth-codeword!  foo codeword 6666 ?s

1234 foo codeword! foo codeword 1234 ?s
6666 foo codeword! foo codeword 6666 ?s

1111 dbg codeword-index 0 ?s
1112 codeword-index 1 ?s
1211 codeword-index 36 ?s
6666 codeword-index 1295 ?s

\ matching
codeword-struct bar

1234 foo codeword! 5656 bar codeword!  foo bar match-result 0 ?s
                   1635 bar codeword!  foo bar match-result 20 ?s
                   1234 bar codeword!  foo bar match-result 40 ?s
                   4321 bar codeword!  foo bar match-result 04 ?s
                   1122 bar codeword!  foo bar match-result 11 ?s
1122 foo codeword! 2616 bar codeword!  foo bar match-result 02 ?s

\ codeword-set structure
foo codeword-set foo-set

\ iterate through a codeword set
foo-set first-codeword!? ?true foo-set dbg current-codeword 1111 ?s 
foo-set next-codeword!?  ?true foo-set current-codeword 1112 ?s
foo-set first-codeword!? ?true
1295 0 [DO] foo-set next-codeword!? ?true [LOOP]
foo-set current-codeword 6666 ?s
foo-set codeword-index 1295 ?s
foo-set next-codeword!? ?false

\ codeword-set member and remove

.s key drop
123 foo-set nth-member? ?true
123 foo-set nth-remove
123 foo-set nth-member? ?true

\ codeword / pegs
1264 pegs-a decompose
pegs-a codeword 1264 ?s
pegs-a pegs-next! ?true pegs-a codeword 1265 ?s
pegs-a pegs-next! ?true pegs-a codeword 1266 ?s
pegs-a pegs-next! ?true pegs-a codeword 1311 ?s
6666 pegs-a decompose
pegs-a pegs-next! ?false


\ codeword values
first-codeword 1111 ?s
last-codeword 6666 ?s
1111 next-codeword 1112 ?s
1116 next-codeword 1121 ?s
1666 next-codeword 2111 ?s
6666 next-codeword 0 ?s

\ codeword-set

codeword-set my-set
my-set set-init!

0 my-set member-or-zero? ?true
1111 my-set member-or-zero? ?true
6666 my-set member-or-zero? ?true

my-set set-length 1296 ?s

my-set first-in-set 1111 ?s
1111 my-set next-in-set 1112 ?s

1122 11 my-set narrow

my-set first-in-set dup 1233 ?s
my-set next-in-set dup 1234 ?s
my-set next-in-set dup 1235 ?s
my-set next-in-set dup 1236 ?s
my-set next-in-set 1243 ?s

my-set set-init!
1234 40 my-set narrow
my-set first-in-set dup 1234 ?s
my-set next-in-set 0 ?s

\ minmax-match-result-score
my-set set-init!
1122 my-set max-match-result-score 256 ?s
1234 my-set max-match-result-score 312 ?s

my-set minmax-match-result-score 1122 ?s

6214 to secret
1122 this decompose secret other decompose
this other match-result 02 ?s
1122 02 my-set guess-move swap 2344 ?s 11 ?s
2344 11 my-set guess-move swap 2415 ?s 12 ?s
2415 12 my-set guess-move swap 2531 ?s 02 ?s
2531 02 my-set guess-move swap 6214 ?s 40 ?s

[THEN]
bye
