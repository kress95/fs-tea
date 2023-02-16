namespace rec App

open Ngn
open Ngn.Platform
open Microsoft.Xna.Framework

type Flags = { None: unit }

type Model = private { Empty: int }

type Msg = internal | TickMsg of GameTime

module Main =

  let application: Game<Flags, Model, Msg> =
    {
      Init = init
      Update = update
      Subscriptions = subscriptions
    }

  let private init flags =
    printfn "INIT"
    ({ Empty = 0 }, Cmd.empty)

  let private update msg model =
    match msg with
    | TickMsg gameTime -> ({ model with Empty = model.Empty + 1 }, Cmd.empty)

  let private subscriptions model = Events.eachTick TickMsg
