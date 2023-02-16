namespace rec Tea.Platform

/// Workers are reducers that can observe external side-effects using subscriptions.
type Worker<'flags, 'model, 'msg, 'cmd, 'sub> =
  {
    /// Initializes the reducer state and produces a side-effect command.
    Init: ('flags -> (struct ('model * 'cmd)))
    /// Updates the reducer state in response to a message and produces a side-effect command.
    Update: ('msg -> 'model -> (struct ('model * 'cmd)))
    /// Specifies the side-effect subscriptions for the program to observe.
    Subscriptions: 'model -> 'sub
  }

[<RequireQualifiedAccess>]
module Worker =

  /// Side-effect performer that executes commands and subscriptions.
  type Perform<'msg, 'cmd, 'sub> =
    {
      /// Evaluate a collection of commands.
      OnCommands: ('msg -> unit) -> 'cmd -> unit
      /// Evaluates a collection of subscriptions.
      OnSubscriptions: ('msg -> unit) -> 'sub -> unit
    }

  /// The mutable state of a worker.
  type Program<'model, 'msg, 'cmd, 'sub>

  /// Initializes the program runtime.
  val init:
    worker: Worker<'flags, 'model, 'msg, 'cmd, 'sub> ->
    perform: Perform<'msg, 'cmd, 'sub> ->
    flags: 'flags ->
      Program<'model, 'msg, 'cmd, 'sub>

  /// Performs a single update step for the program.
  val update: program: Program<'model, 'msg, 'cmd, 'sub> -> unit

  /// Schedules a message to be processed in the next update step.
  val send: program: Program<'model, 'msg, 'cmd, 'sub> -> ('msg -> unit)

  /// Returns the current model of the program.
  val getModel: program: Program<'model, 'msg, 'cmd, 'sub> -> 'model
