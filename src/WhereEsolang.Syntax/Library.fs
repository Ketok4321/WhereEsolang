namespace WhereEsolang.Syntax //TODO: Change name?

type Condition =
    | GreaterThan of uint8
    | LesserThan of uint8
    | Equal of uint8

and Action =
    | Set of uint8
    | Add of uint8
    | Sub of uint8
    | Input of unit
    | Output of unit

and Statement =
    | Where of Condition * Action
    | Loop of Statement list