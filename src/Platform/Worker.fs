//- require "./Reducer.fs"
//- require "./Worker.fsi"

namespace rec Tea.Platform

type Worker<'flags, 'model, 'msg, 'cmd, 'sub> =
  {
    Init: ('flags -> (struct ('model * 'cmd)))
    Update: ('msg -> 'model -> (struct ('model * 'cmd)))
    Subscriptions: 'model -> 'sub
  }

module Worker =

  type Perform<'msg, 'cmd, 'sub> =
    {
      OnCommands: ('msg -> unit) -> 'cmd -> unit
      OnSubscriptions: ('msg -> unit) -> 'sub -> unit
    }

  type Program<'model, 'msg, 'cmd, 'sub> =
    internal
      {
        Reducer: Reducer.Program<'model, 'msg, 'cmd>
        Subscriptions: 'model -> 'sub
        Subscribe: 'sub -> unit
      }

  let init worker perform flags =
    let program =
      Reducer.init
        {
          Init = worker.Init
          Update = worker.Update
        }
        { OnCommands = perform.OnCommands }
        flags

    let subscribe = perform.OnSubscriptions program.Dispatch

    worker.Subscriptions program.Model |> subscribe

    {
      Reducer = program
      Subscriptions = worker.Subscriptions
      Subscribe = subscribe
    }

  let update program =
    let model = program.Reducer.Model

    Reducer.update program.Reducer

    let model' = program.Reducer.Model

    if not <| obj.ReferenceEquals(model, model') then
      program.Subscriptions model' |> program.Subscribe

  let send program = Reducer.send program.Reducer

  let getModel program = Reducer.getModel program.Reducer
