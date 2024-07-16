IDENTIFICATION DIVISION.
PROGRAM-ID. Factorial.

DATA DIVISION.
WORKING-STORAGE SECTION.
01 num            PIC 9(2) VALUE 5.
01 result         PIC 9(18) VALUE 1.

PROCEDURE DIVISION.
    PERFORM FactorialCalc VARYING num FROM num BY -1 UNTIL num = 0
    DISPLAY "The factorial of " num " is " result
    STOP RUN.

FactorialCalc.
    IF num = 0
        MOVE 1 TO result
    ELSE
        MULTIPLY result BY num
