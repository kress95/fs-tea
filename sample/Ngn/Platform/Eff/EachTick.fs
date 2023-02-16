module rec Ngn.Platform.Eff.EachTick

open Tea.Platform
open Microsoft.Xna.Framework

type Program<'msg> = Effect.Program<State<'msg>, 'msg, Msg, MySub<'msg>>

// Init

type State<'msg> = List<GameTime -> 'msg>

let private init () = IO.from List.empty

// Update

type Msg = GameTime

let private onUpdate router gameTime state =
  for toAppMsg in state do
    Router.dispatch router <| toAppMsg gameTime

  IO.from state

// Effect

type MySub<'msg> = (GameTime -> 'msg)

let private onEffect _ subs _ = IO.from subs

// Effect Manager

let effect: Effect<State<'msg>, 'msg, Msg, MySub<'msg>> =
  {
    Init = init
    OnUpdate = onUpdate
    OnEffect = onEffect
  }
