// Program.fs (Stock Market Game Simulation)
// Authors: Sam Lacy and Ognian Trajanov
// Date: 06.04.2025
// Description: Simulates stock price changes based on volatility in a day-by-day game format.

// Usage:
// Have F# installed. (look at the Doc we submitted for part 1 of the project for more details)
// Run "dotnet run" in the terminal
// Enter "buy TICKER #" to buy # stocks
// Enter "sell TICKER #" to sell # stocks
// Press enter to skip day
// Enter "help" to see the available commands
// Enter "quit" to exit the game

module Program

open System
open Types
open DataFetching
open Simulation
open Portfolio
open UI

/// <summary>
/// Our Game Loop (like Void Start in Unity).
/// </summary>
let runGame() =
    // Fetch initial stock data
    printfn "Fetching initial stock data..."
    let mutable stocks = fetchStocksData() |> Async.RunSynchronously
    let mutable day = 1
    
    // Initialize portfolio with starting cash
    let mutable portfolio = { 
        Cash = 10000.0
        Positions = Map.empty 
    }
    
    // Print initial data
    printStocksData stocks portfolio day true
    
    // Game loop
    let mutable running = true
    while running do
        printfn "\nCash: $%.2f | Type help for commands." portfolio.Cash
        let cmd = Console.ReadLine()
        
        // Process command
        let (quitOption, newPortfolio, shouldClearScreen, message) = processCommand cmd portfolio stocks
        portfolio <- newPortfolio
        
        match quitOption with
        | Some quit -> 
            running <- not quit
        | None ->
            let cmdLower = cmd.Trim().ToLower()
            // If just pressing Enter, advance to next day
            if cmd.Trim() = "" then
                day <- day + 1
                
                // Simulate new prices
                stocks <- simulateNewPrices stocks
                
                // Display updated information
                printStocksData stocks portfolio day true
            // Don't refresh display for help command
            elif not (cmdLower = "help") then
                printStocksData stocks portfolio day shouldClearScreen
                // Display any message after the table
                match message with
                | Some msg -> printfn "\n%s" msg
                | None -> ()

// Start the game
[<EntryPoint>]
let main argv =
    runGame()
    0 