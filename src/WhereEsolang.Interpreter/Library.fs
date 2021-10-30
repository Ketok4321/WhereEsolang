namespace WhereEsolang.Interpreter

open System
open WhereEsolang.Syntax

exception TerminationException

module Interpreter =
    let getCondition cond =
        match cond with
        | GreaterThan n -> (fun v -> v > n)
        | LesserThan n -> (fun v -> v < n)
        | Equal n -> (fun v -> v = n)
        
    let getAction act =
        match act with
        | Set n -> (fun v -> n)
        | Add n -> (fun v -> v + n)
        | Sub n -> (fun v -> v - n)
        | Input () -> (fun v -> Console.Write("> "); Console.ReadLine() |> uint8)
        | Output () -> (fun v -> Console.WriteLine(v); v)

    let rec run stmts cells =       
        for stmt in stmts do
            match stmt with
            | Where (cond, act) ->
                let selectedCells = cells |> List.where (fun c -> getCondition cond c.contents)
                for cell in selectedCells do
                    cell.contents <- getAction act cell.contents
                if cells |> List.forall (fun c -> c.contents = 0uy) then
                    raise TerminationException
            | Loop (lstmts) ->
                while true do //TODO: Add some way to break out of loop
                    run lstmts cells