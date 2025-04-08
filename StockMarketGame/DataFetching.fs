// DataFetching.fs
// Functions for fetching stock data from external APIs

module DataFetching

open System.Net.Http
open System.Text.RegularExpressions
open Types

let httpClient = new HttpClient()

// SOURCE: Fancher, D. The Book of F#, p. 245
// USE: Explains how to use asynchronous programming in F# and how to fetch the HTML of a website
/// <summary>
/// Fetches the price of a stock from the CNBC website.
/// </summary>
let fetchPriceAsync (ticker: string) = async {
    let url = $"https://www.cnbc.com/quotes/{ticker}" // Construct the Yahoo Finance URL
    let! html = httpClient.GetStringAsync(url) |> Async.AwaitTask // Fetch HTML asynchronously
    // Find the span with the price
    let m = Regex.Match(html, """<span class="QuoteStrip-lastPrice">([0-9.,]+)</span>""", RegexOptions.IgnoreCase)
    let price = if m.Success then m.Groups.[1].Value else "N/A"

    return ticker, price
}

// SOURCE: Fancher, D. The Book of F#, p. 245
// USE: Explains how to use asynchronous programming in F# and how to fetch the HTML of a website
/// <summary>
/// Fetches the daily high and low values of a stock from the CNBC website.
/// </summary>
let fetchDailyHighLowAsync (ticker: string) = async {
    let url = $"https://www.cnbc.com/quotes/{ticker}" // Construct the Yahoo Finance URL
    let! html = httpClient.GetStringAsync(url) |> Async.AwaitTask // Fetch HTML asynchronously
    
    // Find day low value
    let dayLowMatch = Regex.Match(html, """<li class="Summary-stat"><span class="Summary-label">Day Low</span><span class="Summary-value">([0-9.,]+)</span></li>""", RegexOptions.IgnoreCase)
    let dayLow = if dayLowMatch.Success then dayLowMatch.Groups.[1].Value else "N/A"
    
    // Find day high value
    let dayHighMatch = Regex.Match(html, """<li class="Summary-stat"><span class="Summary-label">Day High</span><span class="Summary-value">([0-9.,]+)</span></li>""", RegexOptions.IgnoreCase)
    let dayHigh = if dayHighMatch.Success then dayHighMatch.Groups.[1].Value else "N/A"
    
    return ticker, dayLow, dayHigh
}

// 9 stock tickers to fetch
let tickers = [
    "AAPL"
    "MSFT"
    "GOOGL"
    "AMZN"
    "META"
    "NVDA"
    "NFLX"
    "INTC"
    "ADBE"
]

/// <summary>
/// Converts a string to a float.
/// </summary>
let parseFloat (s: string) =
    match System.Double.TryParse(s.Replace(",", "")) with
    | true, value -> value
    | _ -> 0.0

/// <summary>
/// Fetches all data and creates stocks (with parallel processing).
/// </summary>
let fetchStocksData() = async {
    // Fetch prices
    let! priceResults = tickers |> List.map fetchPriceAsync |> Async.Parallel
    
    // Fetch high/low values
    let! highLowResults = tickers |> List.map fetchDailyHighLowAsync |> Async.Parallel
    
    // after we fetched everything, we can initialize the beginning of the game
    // note that volatility remains constant (from the day we fetch the data)
    let stocks = 
        Array.zip priceResults highLowResults
        |> Array.map (fun ((ticker, priceStr), (_, lowStr, highStr)) ->
            let price = parseFloat priceStr
            let low = parseFloat lowStr
            let high = parseFloat highStr
            let volatility = (high - low) / low // This has absolutely nothing to do with actual volatility but we're not working witha lot of data
            { 
                Ticker = ticker
                Price = price
                Volatility = volatility
                PreviousPrice = price
                Change = 0.0
            }
        )
    
    return stocks
} 