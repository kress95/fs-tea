namespace rec Tea.Platform

/// Reducers are program definitions that initialize the state of the program and update it in response to messages.
type Reducer<'flags, 'model, 'Msg, 'cmd> =
  {
    /// Initializes the reducer state and produces a side-effect command.
    Init: 'flags -> (struct ('model * 'cmd))
    /// Updates the reducer state in response to a message and produces a side-effect command.
    Update: 'Msg -> 'model -> (struct ('model * 'cmd))
  }

[<RequireQualifiedAccess>]
module Reducer =

  open System.Collections

  /// Side-effect performers.
  type Perform<'Msg, 'cmd> =
    {
      /// Executes a sequence of commands.
      OnCommands: ('Msg -> unit) -> 'cmd -> unit
    }

  /// The mutable state of a reducer.
  type Program<'model, 'Msg, 'cmd> =
    internal
      { mutable Model: 'model
        Update: 'Msg -> 'model -> (struct ('model * 'cmd))
        Mailbox: Generic.Queue<'Msg>
        Dispatch: 'Msg -> unit
        Command: 'cmd -> unit }

  /// Initializes the reducer runtime.
  val init:
    reducer: Reducer<'flags, 'model, 'Msg, 'cmd> ->
    perform: Perform<'Msg, 'cmd> ->
    flags: 'flags ->
      Program<'model, 'Msg, 'cmd>

  /// Performs a single update step for the program.
  val update: program: Program<'model, 'Msg, 'cmd> -> unit

  /// Schedules a message to be processed in the next update step.
  val send: program: Program<'model, 'Msg, 'cmd> -> ('Msg -> unit)

  /// Returns the current model of the program.
  val getModel: program: Program<'model, 'Msg, 'cmd> -> 'model
