namespace Calculatrice_gui

open Avalonia
open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Calculator

module CalculatorView =
    
    // Couleurs personnalisées - conversion correcte
    let backgroundColor = SolidColorBrush(Color.Parse("#1E1E1E"))
    let displayColor = SolidColorBrush(Color.Parse("#2D2D2D"))
    let numberButtonColor = SolidColorBrush(Color.Parse("#3C3C3C"))
    let operatorButtonColor = SolidColorBrush(Color.Parse("#FF9500"))
    let functionButtonColor = SolidColorBrush(Color.Parse("#505050"))
    let textColor = SolidColorBrush(Colors.White)
    
    // Fonction pour créer un bouton stylisé
    let createButton (text: string) (background: SolidColorBrush) (width: float) (height: float) row col msg (dispatch: Msg -> unit) =
        Button.create [
            Button.content text
            Button.width width
            Button.height height
            Button.margin (Thickness(2.0))
            Button.background background
            Button.foreground textColor
            Button.fontSize (if text.Length > 2 then 14.0 else 18.0)
            Button.fontWeight FontWeight.Bold
            Button.horizontalContentAlignment HorizontalAlignment.Center
            Button.verticalContentAlignment VerticalAlignment.Center
            Button.cornerRadius (CornerRadius(5.0))
            Button.onClick (fun _ -> dispatch msg)
            Grid.row row
            Grid.column col
        ] :> IView
    
    // Vue principale
    let view (model: Model) (dispatch: Msg -> unit) =
        let displayText = 
            if model.Error.IsSome then model.Error.Value
            else model.Display
            
        DockPanel.create [
            DockPanel.background backgroundColor
            DockPanel.children [
                // Écran d'affichage
                Border.create [
                    Border.dock Dock.Top
                    Border.background displayColor
                    Border.height 80.0
                    Border.margin (Thickness(5.0))
                    Border.cornerRadius (CornerRadius(5.0))
                    Border.child (
                        TextBlock.create [
                            TextBlock.text displayText
                            TextBlock.fontSize 32.0
                            TextBlock.foreground textColor
                            TextBlock.verticalAlignment VerticalAlignment.Center
                            TextBlock.horizontalAlignment HorizontalAlignment.Right
                            TextBlock.margin (Thickness(10.0, 0.0, 10.0, 0.0))
                        ]
                    )
                ]
                
                // Grille de boutons
                Grid.create [
                    Grid.dock Dock.Top
                    Grid.margin (Thickness(5.0))
                    Grid.rowDefinitions "Auto,Auto,Auto,Auto,Auto,Auto"
                    Grid.columnDefinitions "*,*,*,*"
                    Grid.children [
                        // Première ligne (fonctions) - row 0
                        createButton "C" functionButtonColor 70.0 50.0 0 0 (Clear) dispatch
                        createButton "CE" functionButtonColor 70.0 50.0 0 1 (ClearEntry) dispatch
                        createButton "⌫" functionButtonColor 70.0 50.0 0 2 (Backspace) dispatch
                        createButton "√" operatorButtonColor 70.0 50.0 0 3 (SquareRoot) dispatch
                        
                        // Deuxième ligne - row 1
                        createButton "7" numberButtonColor 70.0 50.0 1 0 (Digit "7") dispatch
                        createButton "8" numberButtonColor 70.0 50.0 1 1 (Digit "8") dispatch
                        createButton "9" numberButtonColor 70.0 50.0 1 2 (Digit "9") dispatch
                        createButton "÷" operatorButtonColor 70.0 50.0 1 3 (Operation "/") dispatch
                        
                        // Troisième ligne - row 2
                        createButton "4" numberButtonColor 70.0 50.0 2 0 (Digit "4") dispatch
                        createButton "5" numberButtonColor 70.0 50.0 2 1 (Digit "5") dispatch
                        createButton "6" numberButtonColor 70.0 50.0 2 2 (Digit "6") dispatch
                        createButton "×" operatorButtonColor 70.0 50.0 2 3 (Operation "*") dispatch
                        
                        // Quatrième ligne - row 3
                        createButton "1" numberButtonColor 70.0 50.0 3 0 (Digit "1") dispatch
                        createButton "2" numberButtonColor 70.0 50.0 3 1 (Digit "2") dispatch
                        createButton "3" numberButtonColor 70.0 50.0 3 2 (Digit "3") dispatch
                        createButton "-" operatorButtonColor 70.0 50.0 3 3 (Operation "-") dispatch
                        
                        // Cinquième ligne - row 4
                        createButton "0" numberButtonColor 70.0 50.0 4 0 (Digit "0") dispatch
                        createButton "00" numberButtonColor 70.0 50.0 4 1 (Digit "00") dispatch
                        createButton "." numberButtonColor 70.0 50.0 4 2 (Decimal) dispatch
                        createButton "+" operatorButtonColor 70.0 50.0 4 3 (Operation "+") dispatch
                        
                        // Sixième ligne - row 5
                        createButton "±" functionButtonColor 70.0 50.0 5 0 (Sign) dispatch
                        createButton "x²" functionButtonColor 70.0 50.0 5 1 (Power) dispatch
                        createButton "1/x" functionButtonColor 70.0 50.0 5 2 (OneOver) dispatch
                        createButton "=" operatorButtonColor 70.0 50.0 5 3 (Equals) dispatch
                    ]
                ]
            ]
        ]
