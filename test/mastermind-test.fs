require ../src/mastermind.fs
require ffl/tst.fs

page

\ binary codewords
1462 as-codeword as-decimal 1462 ?s
6666 as-codeword as-decimal 6666 ?s

\ some values

max-colors 6 ?s max-pegs 4 ?s first-codeword as-decimal 1111 ?s
6 to max-pegs
max-pegs (first-codeword) as-decimal 111111 ?s
max-pegs (last-codeword) as-decimal 666666 ?s
4 to max-pegs

all-match 40 ?s

1111 as-codeword valid-codeword? ?true
4807 as-codeword valid-codeword? ?false

\ first codeword

first-codeword as-decimal 1111 ?s
\ last codeword

last-codeword as-decimal 6666 ?s

\ next codeword

1111 as-codeword next-codeword as-decimal 1112 ?s
1112 as-codeword next-codeword as-decimal 1113 ?s
1116 as-codeword next-codeword as-decimal 1121 ?s
5666 as-codeword next-codeword as-decimal 6111 ?s
6665 as-codeword next-codeword as-decimal 6666 ?s
6666 as-codeword next-codeword ?false
bye

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

