require random.fs

variable secret

: main
    argc @ 2 < if
        10 0 do 1296 random drop loop 
        1296 random .
    else
        1 arg type
    then cr ;

utime drop seed !

main
bye
