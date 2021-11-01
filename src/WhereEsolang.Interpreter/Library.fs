namespace WhereEsolang.Interpreter

open System
open WhereEsolang.Syntax

module Interpreter =
    let createMemoryCells amount =
        [1uy..amount] |> List.map ref
    
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

    let getWhileCondition wcond =
        match wcond with
        | All cond -> List.forall(getCondition cond)
        | Any cond -> List.exists(getCondition cond)
    
    let rec run stmts cells =       
        for stmt in stmts do
            match stmt with
            | Where (cond, act) ->
                let selectedCells = cells |> List.where (fun c -> getCondition cond c.contents)
                for cell in selectedCells do
                    cell.contents <- getAction act cell.contents
            | While (wcond, lstmts) ->
                let wcondFunc = getWhileCondition wcond 
                
                while wcondFunc (cells |> List.map (fun el -> el.contents)) do
                    run lstmts cells