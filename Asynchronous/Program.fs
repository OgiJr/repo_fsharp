// Script Name: Program.fs (Asynchronous)
// Author: Sam Lacy and Ognian Trajanov
// Date: 01.04.2025
// Description: Fetches the title of a website using asynchronous programming.

// Usage:
// Have F# installed. (look at the Doc we submitted for part 1 of the project for more details)
// Run "dotnet run" in the terminal

open System.Net.Http
open System.Text.RegularExpressions

let httpClient = new HttpClient()

// SOURCE: Fancher, D. The Book of F#, p. 245
// USE: Explains how to use asynchronous programming in F# and how to fetch the HTML of a website
let fetchTitleAsync (url: string) = async {
    let! html = httpClient.GetStringAsync(url) |> Async.AwaitTask // Fetches the HTML of the website asynchronously
    let m = Regex.Match(html, "<title>(.*?)</title>", RegexOptions.IgnoreCase) // Finds the title of the website using RegEx
    let title = m.Groups.[1].Value // Return the title
    return url, title
}

let urls = [
    "https://www.bbc.com"
    "https://nova.bg"
    "https://www.cnn.com"
    "https://www.google.com"
    "https://www.youtube.com"
]

// Create a list of async workflows to execute in parallel
let workflows = urls |> List.map fetchTitleAsync

// Run the workflows in parallel
let results = workflows |> Async.Parallel |> Async.RunSynchronously

// Display the results
for (url, title) in results do
    printfn "URL: %-30s | Title: %s" url title
