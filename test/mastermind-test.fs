require ../src/mastermind.fs
require ffl/tst.fs

page

\ some constants

max-colors 6 ?s
max-pegs 4 ?s
max-codewords 1296 ?s \ 6⁴ 

\ number to codeword correspondance
\ 0 ↔ 1111, 1 ↔ 1112, … 1295 ↔ 6666

0 number>codeword 1111 ?s
1 number>codeword 1112 ?s
6 number>codeword 1121 ?s
35 number>codeword 1166 ?s
1295 number>codeword 6666 ?s

1111 codeword>number 0 ?s
1112 codeword>number 1 ?s
6666 codeword>number 1295 ?s

\ number to offset and bits in a set
0 offset,bits 1 ?s 0 ?s
1 offset,bits 2 ?s 0 ?s
2 offset,bits 4 ?s 0 ?s
255 offset,bits 128 ?s 31 ?S
389 offset,bits 32 ?s 48 ?s

\ bitset : inserting, removing, member

create my-set 10 allot
my-set 10 erase

0 my-set bitset-member? ?false
0 my-set bitset-insert
0 my-set bitset-member? ?true
4 my-set bitset-insert
0 my-set bitset-remove
4 my-set bitset-member? ?true
0 my-set bitset-member? ?false

\ codewords set : full, empty, inserting, removing, member, iterate

codewords my-cws
my-cws all-codewords!
my-cws next-codeword 1111 ?s
my-cws next-codeword 1112 ?s
my-cws next-codeword 1113 ?s
1111 my-cws codeword-member? ?true
6666 my-cws codeword-member? ?true
2222 my-cws codeword-member? ?true
2222 my-cws codeword-remove
2222 my-cws codeword-member? ?false
my-cws empty-codewords!
1111 my-cws codeword-member? ?false
6666 my-cws codeword-member? ?false
4444 my-cws codeword-insert
4444 my-cws codeword-member? ?true
1111 my-cws codeword-insert
my-cws codewords-start!
my-cws next-codeword 1111 ?s
my-cws next-codeword 4444 ?s
my-cws next-codeword 0 ?s

\ matches, hits, misses, and match

4264 1234 matches 2 ?s
4264 4264 matches 4 ?s
1111 4264 matches 0 ?s

4264 6314 hits 2 ?s
1264 6314 hits 3 ?s
1264 1264 hits 4 ?s
5555 1264 hits 0 ?s

1264 6314 misses 2 ?s
1264 5555 misses 0 ?s
1264 1264 misses 0 ?s
1122 2211 misses 4 ?s

1264 6314 match 12 ?s
1264 5555 match 00 ?s
1264 1264 match 40 ?s
1122 2211 match 04 ?s

\ maximum of match results for a given codeword
my-cws all-codewords!
1122 my-cws max-match-result 256 ?s

\ codeword with minimum of maximums of match results for a given codeword set
my-cws all-codewords!
my-cws min-max-match-result 1122 ?s

\ guessing the secret codeword
6415 guess-codeword
guess-nb @ 5 ?s
0 guess @ guess>codeword 1122 ?s
0 guess @ guess>result 2 ?s
bye

