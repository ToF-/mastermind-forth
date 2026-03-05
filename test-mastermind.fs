REQUIRE mastermind.fs
REQUIRE ffl/tst.fs

PAGE
T{
    ." index to codeword and back" CR
    0 INDEX>CODEWORD 1111 ?S
    1111 CODEWORD>INDEX 0 ?S
    1295 INDEX>CODEWORD 6666 ?S
    6666 CODEWORD>INDEX 1295 ?S
}T

T{
    ." codeword set iterator" CR
    CODEWORD-SET MY-SET
    MY-SET INITSET
    MY-SET SETSIZE CELL+ DUMP
    MY-SET NEXT-CODEWORD ?true 1111 ?S
    MY-SET NEXT-CODEWORD ?true 1112 ?S
    MY-SET NEXT-CODEWORD ?true 1113 ?S
}T

T{
    ." eliminating a codeword from a set" CR
    MY-SET INITSET
    1112 MY-SET DBG ELIMINATE-CODEWORD
    MY-SET NEXT-CODEWORD ?true 1111 ?S
    MY-SET NEXT-CODEWORD ?true 1113 ?S
}T


BYE
