namespace rec Tea.Platform

/// Effect managers have access to a "router" that routes messages between the running program and itself.
[<Struct>]
type Router<'appMsg, 'selfMsg> =
  internal
    { Dispatch: 'appMsg -> unit
      Loopback: 'selfMsg -> unit }

[<RequireQualifiedAccess>]
module Router =

  /// Sends a message to the running program.
  /// Useful for impure effect managers.
  val dispatch: router: Router<'appMsg, 'selfMsg> -> msg: 'appMsg -> unit

  /// Sends a message back to the EffectManager.
  /// Useful for impure effect managers.
  val loopback: router: Router<'appMsg, 'selfMsg> -> msg: 'selfMsg -> unit

  /// Creates a contained side-effect that sends a message to the running program.
  val sendToApp: router: Router<'appMsg, 'selfMsg> -> msg: 'appMsg -> IO<unit>

  /// Creates a contained side-effect that sends a message back to the EffectManager.
  val sendToSelf: router: Router<'appMsg, 'selfMsg> -> msg: 'selfMsg -> IO<unit>
