namespace rec Ngn.Platform

open Tea.Platform

///  Same as worker, however, it handles specific commands and subscriptions related to game logic.
type Game<'flags, 'model, 'msg> =
  { Init: ('flags -> (struct ('model * Cmd<'msg>)))
    Update: ('msg -> 'model -> (struct ('model * Cmd<'msg>)))
    Subscriptions: 'model -> Sub<'msg> }

[<RequireQualifiedAccess>]
module Game =

  /// Side-effect performer that executes commands and subscriptions.
  type Perform<'msg> =
    {
      /// Evaluate a collection of commands.
      OnCommands: ('msg -> unit) -> List<Cmd.Commands<'msg>> -> unit
      /// Evaluates a collection of subscriptions.
      OnSubscriptions: ('msg -> unit) -> List<Sub.Subscriptions<'msg>> -> unit
    }

  /// The mutable state of a game.
  type Program<'model, 'msg>

  /// Initializes the game runtime.
  val init: game: Game<'flags, 'model, 'msg> -> flags: 'flags -> Program<'model, 'msg>

  /// Initializes the game as a worker.
  /// Only useful for tests.
  val initAsWorker:
    game: Game<'flags, 'model, 'msg> -> perform: Perform<'msg> -> flags: 'flags -> Worker.Program<'model, 'msg>

  /// Executes the game event loop.
  val run: program: Program<'model, 'msg> -> unit

  /// Schedules a message to be processed in the next update step.
  val send: program: Program<'model, 'msg> -> ('msg -> unit)

  /// Returns the current model of the program.
  val getModel: program: Program<'model, 'msg> -> 'model
