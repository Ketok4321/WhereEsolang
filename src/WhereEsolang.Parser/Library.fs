namespace WhereEsolang.Parser

open FParsec
open WhereEsolang.Syntax

module Parser =
    let space : Parser<unit, unit> = skipChar ' '

    let conditionType : Parser<_, unit> = choice [
        charReturn '>' GreaterThan
        charReturn '<' LesserThan
        charReturn '=' Equal
    ]
    
    let condition = conditionType .>>. puint8 |>> fun (func, num) -> func num
    
    module Action =
        let private noParameterAction name =
            skipString name
            
        let private numberParameterAction name =
            noParameterAction name .>> space >>. puint8
        let set = numberParameterAction "SET" |>> Set
        let add = numberParameterAction "ADD" |>> Add
        let sub = numberParameterAction "SUB" |>> Sub
        let input = noParameterAction "INPUT" |>> Input
        let output = noParameterAction "OUTPUT" |>> Output
        
        let any = choice [
            set
            add
            sub
            input
            output
        ]
    
    module Statement =
        let any, anyImpl = createParserForwardedToRef<Statement, unit> ()
        let where = skipString "WHERE" >>. space >>. condition .>> space .>>. Action.any |>> Where
        
        let loopCondition = choice [
            skipString "ANY" >>. space >>. condition |>> Any
            skipString "ALL" >>. space >>. condition |>> All
        ]
        let loop = skipString "WHILE" .>> space >>. loopCondition .>> newline .>>. manyTill (any .>> newline) (skipString "END") |>> While
        
        do anyImpl := spaces >>. choice [
            where
            loop
        ]
    
    let program = (sepEndBy Statement.any newline) .>> spaces .>> eof