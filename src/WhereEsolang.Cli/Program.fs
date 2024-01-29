open System
open System.IO
open FParsec

open WhereEsolang.Parser
open WhereEsolang.Interpreter

[<EntryPoint>]
let main argv =
    match argv with
    | [||] ->
        let cells = Interpreter.createMemoryCells 4uy
        
        while true do
            for cell in cells do
                    Console.Write($"[{cell.contents}]")
            Console.WriteLine()
            
            Console.Write("> ")
            let cmd = Console.ReadLine()
            
            match run (Parser.Statement.any .>> Parser.whitespace) cmd with
            | Success (res, _, _) -> Interpreter.run [res] cells
            | Failure (msg, err, _) -> eprintfn "%s" msg
        1
    | [|path|] ->
        if File.Exists(path) then
            let code = File.ReadAllText(path)

            match run Parser.program code with
            | Success (res, _, _) -> Interpreter.run res (Interpreter.createMemoryCells 4uy); 0
            | Failure (msg, err, _) -> eprintfn "%s" msg; 1
        else
            eprintfn "No such file: %s" path
            1
    | _ ->
        printfn "Usage: whereso [FILE]"
        1
