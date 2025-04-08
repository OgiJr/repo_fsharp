// Script Name: Program.fs (OOP)
// Author: Sam Lacy and Ognian Trajanov
// Date: 01.04.2025
// Description: This program demonstrates polymorphism in F#.

// Usage:
// Have F# installed. (look at the Doc we submitted for part 1 of the project for more details)
// Run "dotnet run" in the terminal

// Our base animal class
type Animal(name: string) =
    member this.Name = name
    abstract member Speak: unit -> string
    default this.Speak() = "AAAAAA"

// SOURCE: Fancher, D. The Book of F#, p. 82
// USAGE: Explains how to implement inheritance in F#
type Dog(name: string) =
    inherit Animal(name)
    override this.Speak() = "Woof!"

type Cat(name: string) =
    inherit Animal(name)
    override this.Speak() = "Meow."

type Snake(name: string) =
    inherit Animal(name)
    override this.Speak() = "Hssss"

// Subtype polymorphism in F#
let describeAnimal (animal: Animal) =
    printfn "%s says: %s" animal.Name (animal.Speak())

// Parametric polymorphism in F#
let printAndReturn (x: 'T) : 'T =
    printfn "You passed: %A" x
    x

// Ad-hoc polymorphism in F# (we don't really have the ability to do ad-hoc the way we do in Java)
let inline handleGeneric input =
    match box input with
    | :? Dog as d -> printfn "Dog handler: %s says %s and wants to play fetch" d.Name (d.Speak()) // do this if the input is a dog
    | :? Cat as c -> printfn "Cat handler: %s says %s and wants to sleep" c.Name (c.Speak()) // do this if the input is a cat
    | :? Snake as s -> printfn "Snake handler: %s says %s and is looking for warmth" s.Name (s.Speak()) // do this if the input is a snake
    | :? Animal as a -> printfn "Generic animal handler: %s makes sound %s" a.Name (a.Speak()) // do this if the input is an animal
    | _ -> printfn "Not an animal: %A" input // do this if the input is not an animal

let dog = Dog("Gerald")
let cat = Cat("Robert")
let snake = Snake("Slimy")


describeAnimal dog
describeAnimal cat
describeAnimal snake

handleGeneric dog
handleGeneric cat
handleGeneric snake

printAndReturn dog