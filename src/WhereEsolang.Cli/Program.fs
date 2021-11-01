open System
open System.IO
open WhereEsolang.Parser
open WhereEsolang.Interpreter
open FParsec

let path = Environment.GetCommandLineArgs().[1]
let code = File.ReadAllText(path)

match run Parser.program code with
| Success (res, _, _) ->
    Interpreter.run res (Interpreter.createMemoryCells 4uy)
| Failure (message, error, _) -> Console.WriteLine(message)