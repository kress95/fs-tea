//- require "./Reducer.fsi"

namespace rec Tea.Platform

type Reducer<'flags, 'model, 'msg, 'cmd> =
  {
    Init: 'flags -> (struct ('model * 'cmd))
    Update: 'msg -> 'model -> (struct ('model * 'cmd))
  }

module Reducer =

  open System.Collections

  type Perform<'msg, 'cmd> =
    {
      OnCommands: ('msg -> unit) -> 'cmd -> unit
    }

  type Program<'model, 'msg, 'cmd> =
    internal
      {
        mutable Model: 'model
        Update: 'msg -> 'model -> (struct ('model * 'cmd))
        Mailbox: Generic.Queue<'msg>
        Dispatch: 'msg -> unit
        Command: 'cmd -> unit
      }

  let init reducer perform flags =
    let mailbox = Generic.Queue()
    let struct (model, cmd) = reducer.Init flags
    let command = perform.OnCommands mailbox.Enqueue

    command cmd

    {
      Model = model
      Update = reducer.Update
      Mailbox = mailbox
      Dispatch = mailbox.Enqueue
      Command = command
    }

  let update program =
    while program.Mailbox.Count > 0 do
      let struct (model, cmd) =
        program.Update (program.Mailbox.Dequeue()) program.Model

      program.Model <- model
      program.Command(cmd)

  let send program = program.Dispatch

  let getModel program = program.Model
