require ../src/mastermind.fs
require ffl/tst.fs

page

\ some values

max-colors 6 ?s
max-pegs 4 ?s

\ match result

1234 5656 match-result 0 ?s
1234 1635 match-result 20 ?s
1234 1234 match-result 40 ?s
1234 4321 match-result 04 ?s
1122 1234 match-result 11 ?s
1122 2616 match-result 02 ?s


bye
