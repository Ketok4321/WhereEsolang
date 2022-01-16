open System
open System.IO
open WhereEsolang.Parser
open WhereEsolang.Interpreter
open FParsec

let args = Environment.GetCommandLineArgs()

if args.Length = 1 then
    let cells = Interpreter.createMemoryCells 4uy
    
    while true do
        for cell in cells do
                Console.Write($"[{cell.contents}]")
        Console.WriteLine()
        
        Console.Write("> ")
        let command = Console.ReadLine()
        
        match run (Parser.Statement.any .>> Parser.whitespace) command with
        | Success (res, _, _) ->
            Interpreter.run [res] cells
        | Failure (message, error, _) -> Console.WriteLine(message)
else
    let code = File.ReadAllText(args.[1])

    match run Parser.program code with
    | Success (res, _, _) ->
        Interpreter.run res (Interpreter.createMemoryCells 4uy)
    | Failure (message, error, _) -> Console.WriteLine(message)