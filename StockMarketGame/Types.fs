// Types.fs
// Type definitions for the Stock Market Game

module Types

/// <summary>
/// a stock with its ticker (id), price, volatility (how much the price can change), previous price, and change.
/// </summary>
type Stock = {
    Ticker: string
    Price: float
    Volatility: float
    PreviousPrice: float
    Change: float
}

/// <summary>
/// a position in a portfolio with the number of shares and their cost.
/// </summary>
type Position = {
    Shares: int
    Cost: float
}

/// <summary>
/// Your portfolio with the amount of cash and the positions in the portfolio.
/// </summary>
type Portfolio = {
    Cash: float
    Positions: Map<string, Position>
} 