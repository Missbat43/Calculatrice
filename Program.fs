namespace Calculatrice_gui

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Elmish
open Elmish
open Calculator
open CalculatorView

type MainWindow() as this =
    inherit HostWindow()
    
    do
        base.Title <- "Calculatrice F#"
        base.Width <- 320.0
        base.Height <- 480.0
        base.MinWidth <- 320.0
        base.MinHeight <- 480.0
        base.CanResize <- false
        
        // Configuration Elmish
        Elmish.Program.mkSimple (fun _ -> initModel) update view
        |> Program.withHost this
        |> Program.run
        
type App() =
    inherit Application()
    
    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark
        
    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktop ->
            desktop.MainWindow <- MainWindow()
        | _ -> ()
        
module Program =
    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .StartWithClassicDesktopLifetime(args)
