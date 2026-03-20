require random.fs
require mastermind.fs

: main
    argc @ 2 < if
        random-codeword to secret
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
        to secret
    then
    guess ;

utime drop seed !
main
bye
