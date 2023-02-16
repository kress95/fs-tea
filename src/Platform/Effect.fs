//- require "./IO.fs"
//- require "./Router.fs"
//- require "./Effect.fsi"

namespace rec Tea.Platform

type Effect<'state, 'appMsg, 'selfMsg, 'eff> =
  {
    Init: unit -> IO<'state>
    OnUpdate: Router<'appMsg, 'selfMsg> -> 'selfMsg -> 'state -> IO<'state>
    OnEffect: Router<'appMsg, 'selfMsg> -> List<'eff> -> 'state -> IO<'state>
  }

[<RequireQualifiedAccess>]
module Effect =

  open System.Collections

  type Program<'state, 'appMsg, 'selfMsg, 'eff> =
    internal
      {
        mutable State: IO<'state>
        OnUpdate: 'selfMsg -> 'state -> IO<'state>
        OnEffect: List<'eff> -> 'state -> IO<'state>
        Mailbox: Generic.Queue<'selfMsg>
        OutMailbox: Generic.Queue<'appMsg>
      }

  let init manager =
    let mailbox = Generic.Queue()
    let outMailbox = Generic.Queue()

    let router: Router<'appMsg, 'selfMsg> =
      {
        Loopback = mailbox.Enqueue
        Dispatch = outMailbox.Enqueue
      }

    {
      State = manager.Init()
      OnUpdate = manager.OnUpdate router
      OnEffect = manager.OnEffect router
      Mailbox = mailbox
      OutMailbox = outMailbox
    }


  let pipe program dispatch =
    while program.OutMailbox.Count > 0 do
      dispatch (program.OutMailbox.Dequeue())

  let update program =
    let mutable state = program.State

    while program.Mailbox.Count > 0 do
      state <- program.OnUpdate(program.Mailbox.Dequeue()) <| IO.perform state

    program.State <- state

  let send program msg = program.Mailbox.Enqueue msg

  let schedule program effs =
    program.State <- program.OnEffect effs <| IO.perform program.State

  let noop
    (_router: Router<'appMsg, 'selfMsg>)
    (_msg: 'selfMsg)
    (state: 'state)
    : IO<'state>
    =
    IO.from state
