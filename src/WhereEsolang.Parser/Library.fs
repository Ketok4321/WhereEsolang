namespace WhereEsolang.Parser

open FParsec
open WhereEsolang.Syntax

module Parser =
    let space : Parser<unit, unit> = skipChar ' '
    let comment : Parser<unit, unit> = skipChar '#' .>> skipRestOfLine false
    let whitespace : Parser<unit, unit> = attempt (skipMany (spaces .>> comment .>> spaces)) <|> spaces
    
    let conditionType : Parser<_, unit> = choice [
        charReturn '>' GreaterThan
        charReturn '<' LesserThan
        charReturn '=' Equal
    ]
    
    let condition = conditionType .>> optional space .>>. puint8 |>> fun (func, num) -> func num
    
    module Action =
        let private noParam name =
            skipString name
        let private numberParam name =
            noParam name .>> space >>. puint8
        
        let set = numberParam "SET" |>> Set
        let add = numberParam "ADD" |>> Add
        let sub = numberParam "SUB" |>> Sub
        let input = noParam "INPUT" >>% Input
        let output = noParam "OUTPUT" >>% Output
        
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
        let loop = skipString "WHILE" .>> space >>. loopCondition .>> newline .>>. manyTill (any .>> newline) (attempt (whitespace .>> skipString "END")) |>> While
        
        let reset = skipString "RESET" >>% Reset
        
        do anyImpl.Value <- whitespace >>. choice [
            where
            loop
            reset
        ]
    
    let program = (sepEndBy Statement.any newline) .>> whitespace .>> eof
