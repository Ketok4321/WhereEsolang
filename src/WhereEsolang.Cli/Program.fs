open System
open System.IO
open WhereEsolang.Parser
open WhereEsolang.Interpreter
open FParsec

let path = Environment.GetCommandLineArgs().[1]
let code = File.ReadAllText(path)

match run Parser.program code with
| Success (res, _, _) ->
    let cells = [1uy..4uy] |> List.map ref
    try
        Interpreter.run res cells
    with
    | TerminationException -> Console.WriteLine "Program was terminated because all cells were set to 0"
| Failure (message, error, _) -> Console.WriteLine(message)