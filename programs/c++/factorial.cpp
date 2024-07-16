#include <iostream>

int factorial(int n) {
    if (n == 0)
        return 1;
    else
        return n * factorial(n - 1);
}

int main() {
    int num = 5;
    std::cout << "The factorial of " << num << " is " << factorial(num) << std::endl;
    return 0;
}
