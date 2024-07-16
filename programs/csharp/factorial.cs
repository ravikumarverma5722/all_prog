using System;

class Factorial
{
    static int FactorialCalc(int n)
    {
        if (n == 0)
            return 1;
        else
            return n * FactorialCalc(n - 1);
    }

    static void Main()
    {
        int num = 5;
        Console.WriteLine("The factorial of " + num + " is " + FactorialCalc(num));
    }
}
