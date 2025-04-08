// Portfolio.fs
// Functions for managing portfolio operations

module Portfolio

open Types

/// <summary>
/// Simulates buying n amount of shares of a stock.
/// </summary>
let buyStock (ticker: string) (shares: int) (portfolio: Portfolio) (stocks: Stock[]) =
    // Find the stock
    let stockOption = stocks |> Array.tryFind (fun s -> s.Ticker = ticker)
    
    // Check if the stock exists
    match stockOption with
    | None -> 
        portfolio, Some (sprintf "'%s' not found in the market." ticker)
    | Some stock ->
        let cost = stock.Price * float shares
        
        // Check if we have enough cash
        if cost > portfolio.Cash then
            portfolio, Some (sprintf "Can't buy %d shares of %s. You need: $%.2f, but only have $%.2f." 
                shares ticker cost portfolio.Cash)
        else
            // Get current position if it exists
            let currentPosition = 
                match Map.tryFind ticker portfolio.Positions with
                | Some pos -> pos
                | None -> { Shares = 0; Cost = 0.0 }
            
            // Calculate new cost
            let totalShares = currentPosition.Shares + shares
            let totalCost = (float currentPosition.Shares * currentPosition.Cost) + cost
            let newCost = if totalShares > 0 then totalCost / float totalShares else 0.0
            
            // Create new position
            let newPosition = { Shares = totalShares; Cost = newCost }
            
            // Update portfolio
            let newPositions = Map.add ticker newPosition portfolio.Positions
            let newCash = portfolio.Cash - cost
            
            let newPortfolio = { Cash = newCash; Positions = newPositions }
            newPortfolio, Some (sprintf "Bought %d shares of %s at $%.2f." 
                shares ticker stock.Price)

// Sell shares of a stock
let sellStock (ticker: string) (shares: int) (portfolio: Portfolio) (stocks: Stock[]) =
    // Find the stock
    let stockOption = stocks |> Array.tryFind (fun s -> s.Ticker = ticker)
    
    match stockOption with
    | None -> 
        portfolio, Some (sprintf "'%s' not found." ticker)
    | Some stock ->
        // Check if we have the position and enough shares
        match Map.tryFind ticker portfolio.Positions with
        | None -> 
            portfolio, Some (sprintf "You don't own any shares of %s" ticker)
        | Some position ->
            if position.Shares < shares then
                portfolio, Some (sprintf "Can't sell %d shares of %s. You only have %d shares." 
                    shares ticker position.Shares)
            else
                // Calculate proceeds
                let proceeds = stock.Price * float shares
                
                // Create new position
                let newShares = position.Shares - shares
                let newPosition = 
                    if newShares > 0 then
                        Some { position with Shares = newShares }
                    else
                        None
                
                // Update portfolio
                let newPositions = 
                    match newPosition with
                    | Some pos -> Map.add ticker pos portfolio.Positions
                    | None -> Map.remove ticker portfolio.Positions
                    
                let newCash = portfolio.Cash + proceeds
                
                let profit = proceeds - (float shares * position.Cost)
                
                let newPortfolio = { Cash = newCash; Positions = newPositions }
                newPortfolio, Some (sprintf "Successfully sold %d shares of %s at $%.2f per share\n   - Total proceeds: $%.2f\n   - Profit/Loss: $%.2f\n   - New cash balance: $%.2f" 
                    shares ticker stock.Price proceeds profit newCash) 