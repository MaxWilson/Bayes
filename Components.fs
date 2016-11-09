module Components

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Fable.Import.React
open Fable.Import.React_Extensions

module R = Fable.Helpers.React
open R.Props

type AppState = { prior : int }

type App() as this =
    inherit Component<unit, AppState>()
    do this.state <- { prior = 50 }
    let computePosterior prior =
        let ph = (prior |> float) / 100.
        let peh = 0.29
        let pe = (peh * ph + 0.5 * (1. - ph))
        peh * ph / pe * 100. |> int
    let handleChange (e: FormEvent) =
        let txt = e.target?value |> unbox<string>
        let mutable v = JS.parseFloat txt |> int
        if v < 0 then v <- 0
        if v > 100 then v <- 100
        this.setState { prior = v }
    member this.render() =
        R.div [] [
            R.div [Style [CSSProp.FontSize (U2.Case2 "24px")]][R.str "How much confidence (%) did you have in 538's predictions before?"]
            R.input [Value (U2.Case1 (sprintf "%d" this.state.prior)); OnChange handleChange][]
            R.div [Style [CSSProp.FontSize (U2.Case2 "24px")]][R.str (sprintf "If you originally had %d%% confidence in 538's predictions, after Trump's win you should now rationally have %d%% confidence in them." this.state.prior (computePosterior this.state.prior))]
            ]