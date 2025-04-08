// Simulation.fs
// Functions for simulating stock market behavior

module Simulation

open System
open Types

/// <summary>
/// Generates new prices based on constant volatility.
/// </summary>
let simulateNewPrices (stocks: Stock[]) =
    let random = Random()
    
    stocks 
    |> Array.map (fun stock ->
        // For each stock, determine the maximum possible change so the game doesn't go too crazy
        let maxChange = stock.Volatility * 2.0
        
        // Generate random change between -maxChange and +maxChange
        let randomFactor = (random.NextDouble() * 2.0 - 1.0) * maxChange
        
        // Calculate new price (ensure it doesn't go below 0)
        let newPrice = max 0.01 (stock.Price * (1.0 + randomFactor))
        
        // Calculate percentage change from previous price
        let changePercentage = (newPrice - stock.Price) / stock.Price
        
        // Return updated stock
        { 
            stock with 
                PreviousPrice = stock.Price
                Price = newPrice
                Change = changePercentage
        }
    )

/// <summary>
/// Calculates the total portfolio value based on current stock prices.
/// </summary>
let calculatePortfolioValue (portfolio: Portfolio) (stocks: Stock[]) =
    let stocksMap = stocks |> Array.map (fun s -> s.Ticker, s) |> Map.ofArray
    
    let stocksValue = 
        portfolio.Positions
        |> Map.toSeq
        |> Seq.sumBy (fun (ticker, position) ->
            match Map.tryFind ticker stocksMap with
            | Some stock -> float position.Shares * stock.Price
            | None -> 0.0
        )
    
    portfolio.Cash + stocksValue 