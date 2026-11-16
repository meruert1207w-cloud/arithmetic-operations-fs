module Arithmetic

open System

let private parseOperands (operationName: string) (values: obj array) : Result<float * float, string> =
    if isNull values then
        Error $"{operationName}: кіріс массиві null болмауы керек."
    elif values.Length <> 2 then
        Error $"{operationName}: дәл екі сан қажет."
    else
        let parseValue index (value: obj) =
            if isNull value then
                Error $"{operationName}: {index + 1}-мән null/undefined болмауы керек."
            else
                let parsed =
                    match value with
                    | :? int as number -> Some(float number)
                    | :? int64 as number -> Some(float number)
                    | :? float as number -> Some number
                    | :? single as number -> Some(float number)
                    | :? decimal as number -> Some(float number)
                    | _ -> None

                match parsed with
                | None -> Error $"{operationName}: {index + 1}-мән сан болуы керек (мәтін қабылданбайды)."
                | Some number when Double.IsNaN number || Double.IsInfinity number ->
                    Error $"{operationName}: шекті сан мәні қажет."
                | Some number when number < 0.0 ->
                    Error $"{operationName}: теріс мәндерге рұқсат жоқ."
                | Some number -> Ok number

        match parseValue 0 values[0], parseValue 1 values[1] with
        | Ok first, Ok second -> Ok(first, second)
        | Error message, _ -> Error message
        | _, Error message -> Error message

let add : obj array -> Result<float, string> = function
    | values ->
        match parseOperands "Қосу" values with
        | Error message -> Error message
        | Ok(first, second) -> Ok(first + second)

let subtract : obj array -> Result<float, string> = function
    | values ->
        match parseOperands "Азайту" values with
        | Error message -> Error message
        | Ok(first, second) when first < second -> Error "Азайту: нәтиже теріс болмауы керек."
        | Ok(first, second) -> Ok(first - second)

let multiply : obj array -> Result<float, string> = function
    | values ->
        match parseOperands "Көбейту" values with
        | Error message -> Error message
        | Ok(first, second) -> Ok(first * second)

let divide : obj array -> Result<float, string> = function
    | values ->
        match parseOperands "Бөлу" values with
        | Error message -> Error message
        | Ok(_, 0.0) -> Error "Бөлу: нөлге бөлуге болмайды."
        | Ok(first, second) -> Ok(first / second)
