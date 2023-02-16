//- require "./Cmd.fs"
//- require "./Eff/EachTick.fs"
//- require "./Effects.fs"
//- require "./Loader.fs"
//- require "./Runtime.fs"
//- require "./Sub.fs"
//- require "./Worker.fs"
//- require "./Game.fsi"

namespace rec Ngn.Platform

open Tea.Platform

type Game<'flags, 'model, 'msg> =
  {
    Init: ('flags -> (struct ('model * Cmd<'msg>)))
    Update: ('msg -> 'model -> (struct ('model * Cmd<'msg>)))
    Subscriptions: 'model -> Sub<'msg>
  }

[<RequireQualifiedAccess>]
module Game =

  open Ngn.Platform.Eff

  type Perform<'msg> =
    {
      /// Evaluate a collection of commands.
      OnCommands: ('msg -> unit) -> List<Cmd.Commands<'msg>> -> unit
      /// Evaluates a collection of subscriptions.
      OnSubscriptions: ('msg -> unit) -> List<Sub.Subscriptions<'msg>> -> unit
    }

  type Program<'model, 'msg> = private | Program of Runtime<'model, 'msg>

  let init game flags =
    Loader.unsafeLoad ()

    let eachTick = Effect.init EachTick.effect

    let effects: Effects<'msg> =
      {
        OnEachTick = Effect.schedule eachTick
      }

    let perform: Perform<'msg> =
      {
        OnCommands = Cmd.unsafePerform effects
        OnSubscriptions = Sub.unsafePerform effects
      }

    let worker = initAsWorker game perform flags

    Program(new Runtime<'model, 'msg> { App = worker; EachTick = eachTick })

  let initAsWorker game perform flags =
    let init flags =
      let struct (model, cmd) = game.Init flags
      (struct (model, Cmd.unwrap cmd))

    let update msg model =
      let struct (model, cmd) = game.Update msg model
      (struct (model, Cmd.unwrap cmd))

    let subscriptions model = Sub.unwrap <| game.Subscriptions model

    Worker.init
      {
        Init = init
        Update = update
        Subscriptions = subscriptions
      }
      {
        OnCommands = perform.OnCommands
        OnSubscriptions = perform.OnSubscriptions
      }
      flags


  let run program =
    let (Program runtime) = program
    runtime.Run()

  let send (program: Program<'model, 'msg>) : ('msg -> unit) =
    let (Program runtime) = program
    Worker.send runtime.programs.App

  let getModel (program: Program<'model, 'msg>) : 'model =
    let (Program runtime) = program
    Worker.getModel runtime.programs.App
