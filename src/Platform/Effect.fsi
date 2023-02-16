namespace rec Tea.Platform

/// Effects are program definitions that manage side-effects scheduled by other programs.
/// In contrast to reducers and workers, the callbacks defined in this context are allowed to perform unmanaged side-effects.
/// However, it is still recommended to utilize an IO monad with chained side-effects.
type Effect<'state, 'appMsg, 'selfMsg, 'eff> =
  {
    /// Initialize the effect manager state.
    Init: unit -> IO<'state>
    /// Update the state in response to an internal message.
    OnUpdate: Router<'appMsg, 'selfMsg> -> 'selfMsg -> 'state -> IO<'state>
    /// Handle scheduled side-effects.
    OnEffect: Router<'appMsg, 'selfMsg> -> List<'eff> -> 'state -> IO<'state>
  }

[<RequireQualifiedAccess>]
module Effect =

  /// The mutable state of an effect.
  type Program<'state, 'appMsg, 'selfMsg, 'eff>

  /// Initializes the effect runtime.
  val init: manager: Effect<'state, 'appMsg, 'selfMsg, 'eff> -> Program<'state, 'appMsg, 'selfMsg, 'eff>

  /// Pipe events to the runtime representation of the EffectManager.
  val pipe: program: Program<'state, 'appMsg, 'selfMsg, 'eff> -> dispatch: ('appMsg -> unit) -> unit

  /// Performs a single update step for the effect program.
  val update: program: Program<'state, 'appMsg, 'selfMsg, 'eff> -> unit

  /// Schedules a message to be processed in the next update step.
  val send: program: Program<'state, 'appMsg, 'selfMsg, 'eff> -> msg: 'selfMsg -> unit

  /// Schedule side-effects to be processed in the next update step.
  val schedule: program: Program<'state, 'appMsg, 'selfMsg, 'eff> -> effs: List<'eff> -> unit

  /// No operation, used as a fallback for the `OnUpdate` callback.
  val noop: _router: Router<'appMsg, 'selfMsg> -> _msg: 'selfMsg -> state: 'state -> IO<'state>
