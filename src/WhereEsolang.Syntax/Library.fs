namespace WhereEsolang.Syntax //TODO: Change name?

type Condition =
    | GreaterThan of uint8
    | LesserThan of uint8
    | Equal of uint8

type Action =
    | Set of uint8
    | Add of uint8
    | Sub of uint8
    | Input of unit
    | Output of unit

type WhileCondition =
    | Any of Condition
    | All of Condition

type Statement =
    | Where of Condition * Action
    | While of WhileCondition * Statement list