// Script Name: Program.fs (Fibonacci)
// Author: Sam Lacy and Ognian Trajanov
// Date: 01.04.2025
// Description: Calculates the N-th Fibonacci using both tail and non-tail recursion.

// Usage:
// Have F# installed. (look at the Doc we submitted for part 1 of the project for more details)
// Run "dotnet run" in the terminal
// Change the value of n but make sure it's less than 47 and greater than 0 (to avoid integer overflow)


// SOURCE: Fancher, D. The Book of F#, p. 109
// USE: Explains non-tail recursion in F# and how to implement it
/// <summary>
/// Calculates the n-th Fibonacci number using non-tail recursion.
/// </summary>
let rec fibonacci n = // F# has a rec keyword to define recursive functions
    match n with  // Similar to a switch in Java
    | 0 -> 0 // Base case 1
    | 1 -> 1 // Base case 2
    | _ -> fibonacci (n - 1) + fibonacci (n - 2) // Recursive case; See how addition happens after the recursive call (Non-Tail)

// SOURCE: Fancher, D. The Book of F#, p. 110
// USE: Explains tail-recursion in F#
/// <summary>
/// Calculates the n-th Fibonacci number using tail recursion.
/// </summary>
let fibonacciTail n =
    // Helper function that performs the recursion
    let rec loop a b count =
        if count = 0 then a // Base case returns the first number
        else loop b (a + b) (count - 1) // Recursive case; See how the recursive call is the last operation
    loop 0 1 n // calls the helper function with the initial values

let n = 20
let run = fibonacci(n) |> printfn"%i"
let runTail = fibonacciTail(n) |> printfn"%i"