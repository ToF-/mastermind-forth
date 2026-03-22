require ../src/mastermind.fs
require ffl/tst.fs

page

\ some values

max-colors 6 ?s max-pegs 4 ?s first-codeword 1111 ?s
6 (first-codeword) 111111 ?s

all-match 40 ?s

1111 valid-codeword? ?true
4807 valid-codeword? ?false

\ last codeword

last-codeword 6666 ?s

\ next codeword

1111 next-codeword 1112 ?s
1112 next-codeword 1113 ?s
1116 next-codeword 1121 ?s
5666 next-codeword 6111 ?s
6665 next-codeword 6666 ?s
6666 next-codeword ?false

\ color tally

1246 color-tally 101011 ?s
1122 color-tally 000022 ?s
6666 color-tally 400000 ?s
3335 color-tally 010300 ?s

\ hits : number of colors found, regardless of position

1246 1126 hits 3 ?s
6666 6666 hits 4 ?s
1234 5555 hits 0 ?s
4444 1236 hits 0 ?s

\ matches : number of colors found in correct position

1246 1345 matches 2 ?s
1234 1234 matches 4 ?s
1111 2345 matches 0 ?s
1234 4321 matches 0 ?s
1111 1222 matches 1 ?s

\ match : matches * 10 + misses

1246 1126 match 21 ?s
1234 5555 match 00 ?s
1122 1234 match 11 ?s
1111 3146 match 10 ?s
1234 4321 match 04 ?s

\ intervals 

intervals foo
foo max-intervals 0 ?s
1 200 foo add-interval
foo max-intervals 1 ?s
foo current-interval 200 ?s 1 ?s
345 452 foo add-interval 
foo max-intervals 2 ?s
foo current-interval 452 ?s 345 ?s

foo initial-interval
foo max-intervals 1 ?s
foo current-interval 6666 ?s 1111 ?s

\ given guesses already made, what is the first possible codeword, and the next ones ? 
max-guesses off

first-candidate 1111 ?s

1122 10 add-guess
1344 11 add-guess

1111 0 guess#-match? ?false

1111 in-solution? ?false
1535 in-solution? ?true

first-candidate 1455 ?s
1455 next-candidate 1456 ?s

4524 12 add-guess
first-candidate 3542 ?s
3542 next-candidate 4352 ?s
4352 next-candidate 4442 ?s

1356 02 add-guess

first-candidate 3542 ?s
3542 next-candidate 0 ?s

max-guesses off
1122 20 add-guess
1234 12 add-guess
1352 21 add-guess
1323 11 add-guess
first-candidate 1542 ?s

\ max match result for a given codeword and a given solution space
max-guesses off
1122 max-match-result-score 256 ?s

\ min max match results for all codewords and a given solution space
min-max-match-result-score 1122 ?s

\ guessing in 5 moves or less
guess
max-guesses @ 5 ?s
.guesses

bye

