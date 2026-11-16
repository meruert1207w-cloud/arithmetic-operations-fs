module Arithmetic

open System

type Functions =
    static member private parseValue : string * int * obj -> Result<float, string> = function
        | operationName, index, value when isNull value ->
            Error $"{operationName}: {index + 1}-мән null/undefined болмауы керек."
        | operationName, index, value ->
            match value with
            | :? int as number -> Ok(float number)
            | :? int64 as number -> Ok(float number)
            | :? float as number -> Ok number
            | :? single as number -> Ok(float number)
            | :? decimal as number -> Ok(float number)
            | _ -> Error $"{operationName}: {index + 1}-мән сан болуы керек (мәтін қабылданбайды)."

    static member private validateValue : string * int * Result<float, string> -> Result<float, string> = function
        | operationName, _, Error message -> Error message
        | operationName, _, Ok number when Double.IsNaN number || Double.IsInfinity number ->
            Error $"{operationName}: шекті сан мәні қажет."
        | operationName, _, Ok number when number < 0.0 ->
            Error $"{operationName}: теріс мәндерге рұқсат жоқ."
        | _, _, Ok number -> Ok number

    static member private parseOperands : string * obj array -> Result<float * float, string> = function
        | operationName, values when isNull values ->
            Error $"{operationName}: кіріс массиві null болмауы керек."
        | operationName, values when values.Length <> 2 ->
            Error $"{operationName}: дәл екі сан қажет."
        | operationName, values ->
            match
                Functions.validateValue (operationName, 0, Functions.parseValue (operationName, 0, values[0])),
                Functions.validateValue (operationName, 1, Functions.parseValue (operationName, 1, values[1]))
            with
            | Ok first, Ok second -> Ok(first, second)
            | Error message, _ -> Error message
            | _, Error message -> Error message

    static member add (a: obj, b: obj) : Result<float, string> =
        match Functions.parseOperands ("Қосу", [| a; b |]) with
        | Error message -> Error message
        | Ok(first, second) -> Ok(first + second)

    static member subtract : obj array -> Result<float, string> = function
        | values ->
            match Functions.parseOperands ("Азайту", values) with
            | Error message -> Error message
            | Ok(first, second) when first < second -> Error "Азайту: нәтиже теріс болмауы керек."
            | Ok(first, second) -> Ok(first - second)

    static member multiply : obj array -> Result<float, string> = function
        | values ->
            match Functions.parseOperands ("Көбейту", values) with
            | Error message -> Error message
            | Ok(first, second) -> Ok(first * second)

    static member divide : obj array -> Result<float, string> = function
        | values ->
            match Functions.parseOperands ("Бөлу", values) with
            | Error message -> Error message
            | Ok(_, 0.0) -> Error "Бөлу: нөлге бөлуге болмайды."
            | Ok(first, second) -> Ok(first / second)

