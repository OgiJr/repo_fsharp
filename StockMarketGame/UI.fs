// UI.fs
// Functions for user interface and interaction

module UI

open System
open Types
open Simulation
open Portfolio

/// <summary>
/// Prints the stocks data with portfolio information.
/// </summary>
let printStocksData (stocks: Stock[]) (portfolio: Portfolio) (day: int) (clearScreen: bool) =
    if clearScreen then
        Console.Clear()
    
    // Calculate total portfolio value
    let portfolioValue = calculatePortfolioValue portfolio stocks
    
    // Print header with portfolio summary
    printfn "\nStock Market Game - Day %d" day
    printfn "Net Worth: $%.2f\n" portfolioValue
    
    printfn "%-6s | %-10s | %-10s | %-10s | %-8s | %-12s | %-12s" 
        "Ticker" "Price" "Change" "Volatility" "Shares" "Value" "Profit/Loss"
    printfn "------------------------------------------------------------------------------"
    
    let stocksMap = stocks |> Array.map (fun s -> s.Ticker, s) |> Map.ofArray
    
    for stock in stocks do
        // Set color based on price change
        if stock.Change > 0.0 then
            Console.ForegroundColor <- ConsoleColor.Green
        elif stock.Change < 0.0 then
            Console.ForegroundColor <- ConsoleColor.Red
        else
            Console.ForegroundColor <- ConsoleColor.Gray
            
        // Get position information if we own this stock
        let position = Map.tryFind stock.Ticker portfolio.Positions
        let shares = match position with Some p -> p.Shares | None -> 0
        let value = float shares * stock.Price
        
        // Calculate profit/loss
        let profitLoss = 
            match position with
            | Some p -> value - (float p.Shares * p.Cost)
            | None -> 0.0
        
        // Set profit/loss color
        let originalColor = Console.ForegroundColor
        
        // Print stock data
        printfn "%-6s | $%-9.2f | %+7.2f%% | %.2f%% | %-8d | $%-11.2f | " 
            stock.Ticker 
            stock.Price 
            (stock.Change * 100.0) 
            (stock.Volatility * 100.0)
            shares
            value
            
        // Print profit/loss with color
        if profitLoss > 0.0 then
            Console.ForegroundColor <- ConsoleColor.Green
            printf "$+%.2f" profitLoss
        elif profitLoss < 0.0 then
            Console.ForegroundColor <- ConsoleColor.Red
            printf "$%.2f" profitLoss
        else
            printf "$0.00"
            
        // Reset color and end line
        Console.ForegroundColor <- ConsoleColor.Gray
        printfn ""
            
    // Reset color
    Console.ForegroundColor <- ConsoleColor.Gray

// Process user command
let processCommand (cmd: string) (portfolio: Portfolio) (stocks: Stock[]) =
    let parts = cmd.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
    
    if parts.Length = 0 then
        // Just pressing Enter - advance to next day
        None, portfolio, true, None
    else
        match parts.[0].ToLower() with
        | "quit" -> 
            // Quit the game
            Some false, portfolio, true, None
        | "buy" when parts.Length >= 3 ->
            // Buy command: b TICKER SHARES
            let ticker = parts.[1].ToUpper()
            match Int32.TryParse(parts.[2]) with
            | true, shares when shares > 0 ->
                let (newPortfolio, message) = buyStock ticker shares portfolio stocks
                None, newPortfolio, (newPortfolio <> portfolio), message
            | _ ->
                None, portfolio, false, Some "Invalid number of shares. Usage: buy TICKER SHARES"
        | "sell" when parts.Length >= 3 ->
            // Sell command: s TICKER SHARES
            let ticker = parts.[1].ToUpper()
            match Int32.TryParse(parts.[2]) with
            | true, shares when shares > 0 ->
                let (newPortfolio, message) = sellStock ticker shares portfolio stocks
                None, newPortfolio, (newPortfolio <> portfolio), message
            | _ ->
                None, portfolio, false, Some "Invalid number of shares. Usage: sell TICKER SHARES"
        | "help" ->
            // Print help
            None, portfolio, false, Some """
Commands:
  [Enter]       - Advance to next day
  buy TICKER N  - Buy N shares of TICKER
  sell TICKER N - Sell N shares of TICKER
  help          - Show this help
  quit          - Exit the game"""
        | _ ->
            None, portfolio, false, Some "Unknown command. Type 'help' for available commands." 