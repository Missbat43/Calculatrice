namespace Calculatrice_gui

open System

module Calculator =
    // Modèle de données (immuable)
    type Model = {
        Display: string
        CurrentValue: float
        PreviousValue: float
        Operation: string option
        NewNumber: bool
        Error: string option
    }

    // État initial
    let initModel = {
        Display = "0"
        CurrentValue = 0.0
        PreviousValue = 0.0
        Operation = None
        NewNumber = true
        Error = None
    }

    // Messages pour les actions utilisateur
    type Msg =
        | Digit of string
        | Operation of string
        | Equals
        | Clear
        | ClearEntry
        | Decimal
        | Sign
        | Backspace
        | SquareRoot
        | Power
        | OneOver

    // Fonctions mathématiques pures
    let add x y = x + y
    let subtract x y = x - y
    let multiply x y = x * y
    
    let divide x y = 
        if y = 0.0 then Error "Division par zéro"
        else Ok (x / y)

    let squareRoot x =
        if x < 0.0 then Error "Racine carrée d'un négatif"
        else Ok (sqrt x)

    let percent x y = x * y / 100.0

    // Fonction de mise à jour (pure)
    let update (msg: Msg) (model: Model) : Model =
        try
            match msg with
            | Digit d ->
                if model.NewNumber || model.Display = "0" || model.Display = "-0" then
                    { model with 
                        Display = if d = "0" && model.Display = "0" then "0" else d
                        NewNumber = false
                        Error = None }
                else
                    { model with 
                        Display = model.Display + d
                        Error = None }
            
            | Decimal ->
                if model.NewNumber then
                    { model with Display = "0."; NewNumber = false }
                elif not (model.Display.Contains ".") then
                    { model with Display = model.Display + "." }
                else model
            
            | Clear -> initModel
            
            | ClearEntry ->
                { model with Display = "0"; NewNumber = true }
            
            | Sign ->
                if model.Display <> "0" && model.Display <> "-0" then
                    let value = float model.Display
                    { model with Display = sprintf "%g" (-value) }
                else model
            
            | Backspace ->
                if model.Display.Length > 1 && model.Display <> "-0" then
                    { model with Display = model.Display.[0..model.Display.Length-2] }
                else
                    { model with Display = "0"; NewNumber = true }
            
            | Operation op ->
                let currentValue = float model.Display
                match model.Operation with
                | Some prevOp when not model.NewNumber ->
                    // Calculer l'opération précédente
                    let result =
                        match prevOp with
                        | "+" -> add model.PreviousValue currentValue
                        | "-" -> subtract model.PreviousValue currentValue
                        | "*" -> multiply model.PreviousValue currentValue
                        | "/" -> 
                            match divide model.PreviousValue currentValue with
                            | Ok res -> res
                            | Error _ -> 0.0
                        | _ -> currentValue
                    
                    { model with
                        PreviousValue = result
                        Display = sprintf "%g" result
                        Operation = Some op
                        NewNumber = true
                        CurrentValue = result }
                | _ ->
                    { model with
                        PreviousValue = currentValue
                        Operation = Some op
                        NewNumber = true
                        CurrentValue = currentValue }
            
            | Equals ->
                let currentValue = float model.Display
                match model.Operation with
                | Some op ->
                    let result =
                        match op with
                        | "+" -> add model.PreviousValue currentValue
                        | "-" -> subtract model.PreviousValue currentValue
                        | "*" -> multiply model.PreviousValue currentValue
                        | "/" -> 
                            match divide model.PreviousValue currentValue with
                            | Ok res -> res
                            | Error msg -> 
                                { model with Error = Some msg; Display = "Erreur" } |> ignore
                                0.0
                        | _ -> currentValue
                    
                    { model with
                        Display = sprintf "%g" result
                        PreviousValue = result
                        CurrentValue = result
                        Operation = None
                        NewNumber = true
                        Error = None }
                | _ -> model
            
            | SquareRoot ->
                let value = float model.Display
                match squareRoot value with
                | Ok res ->
                    { model with
                        Display = sprintf "%g" res
                        CurrentValue = res
                        NewNumber = true }
                | Error msg ->
                    { model with Error = Some msg; Display = "Erreur" }
            
            | Power ->
                // x^2 (carré)
                let value = float model.Display
                let result = value * value
                { model with
                    Display = sprintf "%g" result
                    CurrentValue = result
                    NewNumber = true }
            
            | OneOver ->
                let value = float model.Display
                if value = 0.0 then
                    { model with Error = Some "Division par zéro"; Display = "Erreur" }
                else
                    let result = 1.0 / value
                    { model with
                        Display = sprintf "%g" result
                        CurrentValue = result
                        NewNumber = true }
        with
        | ex -> { model with Error = Some ex.Message; Display = "Erreur" }
