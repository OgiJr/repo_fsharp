// Program Name: Program.fs (Queries)
// Author: Sam Lacy and Ognian Trajanov
// Date: 01.04.2025
// Description: Demonstrates how to query a dataset using F#'s sequence expressions (similar to GO and MongoDB but worse).

// Usage:
// Have F# installed. (look at the Doc we submitted for part 1 of the project for more details)
// Run "dotnet run" in the terminal

// SOURCE: Fancher, D. The Book of F#, p. 202
// USE: Explains how to do queries in F#
type Student = {
    ID: int
    Name: string
    Major: string
    GPA: float
    Year: int
}

// Sample data: list of student records
let students = [
    { ID = 1; Name = "Samuel Lacy"; Major = "Computer Science"; GPA = 4.0; Year = 2027 }
    { ID = 2; Name = "Ognian Trajanov"; Major = "Computer Science"; GPA = 4.0; Year = 2026 }
    { ID = 3; Name = "Johnatan Doe"; Major = "Computer Science"; GPA = 2.8; Year = 2025 }
    { ID = 4; Name = "Jenifer Doe"; Major = "Physics"; GPA = 3.7; Year = 2027 }
    { ID = 5; Name = "Steven Segal"; Major = "Mathematics"; GPA = 3.2; Year = 2026 }
    { ID = 6; Name = "William Smith"; Major = "Biology"; GPA = 3.9; Year = 2025 }
    { ID = 7; Name = "William Gates"; Major = "Computer Science"; GPA = 3.6; Year = 2026 }
    { ID = 8; Name = "Stephen Jobs"; Major = "Physics"; GPA = 3.8; Year = 2025 }
    { ID = 9; Name = "Marcus Zuckerberg"; Major = "Mathematics"; GPA = 2.9; Year = 2027 }
]

// Query 1: Students with GPA above or equal to 3.5
let highGPAStudents =
    students
    |> List.filter (fun s -> s.GPA >= 3.5)
    |> List.map (fun s -> $"{s.Name} ({s.Major}) - GPA: {s.GPA}")

printfn "Students with GPA >= 3.5:"
highGPAStudents |> List.iter (printfn "%s")

printfn ""

// Query 2: Computer Science majors graduating in 2026 or 2027
let csGraduating =
    seq {
        for s in students do
            if s.Major = "Computer Science" && (s.Year = 2026 || s.Year = 2027) then
                yield $"{s.Name} - ID: {s.ID}, GPA: {s.GPA}"
    }

printfn "CS Students graduating in 2026 or 2027:"
csGraduating |> Seq.iter (printfn "%s")

printfn ""

// Query 3: Average GPA of all students
let avgGPA =
    students
    |> List.averageBy (fun s -> s.GPA)

printfn "Average GPA: %.2f" avgGPA