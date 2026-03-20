require ../src/mastermind.fs
require ffl/tst.fs

page

\ some values

max-colors 6 ?s max-pegs 4 ?s first-codeword 1111 ?s
6 (first-codeword) 111111 ?s

\ last-codeword

last-codeword 6666 ?s

\ next codeword

1111 next-codeword 1112 ?s
1112 next-codeword 1113 ?s
1116 next-codeword 1121 ?s



bye

