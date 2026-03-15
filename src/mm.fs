require random.fs
require mastermind.fs

: main
    argc @ 2 < if
        max-codewords random number>codeword
    else
        1 arg s>number? if
            drop
            dup valid-codeword? 0 = if
                ." not a valid codeword"
                bye
            then
        else
            ." not a number"
            bye
        then
    then
    secret !
    guess-codeword
    .results ;

utime drop seed !

main
bye
