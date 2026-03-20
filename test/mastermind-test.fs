require ../src/mastermind.fs
require ffl/tst.fs

page

\ some values

max-colors 6 ?s max-pegs 4 ?s first-codeword 1111 ?s
6 (first-codeword) 111111 ?s

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

bye

