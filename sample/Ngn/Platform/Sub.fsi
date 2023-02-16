namespace rec Ngn.Platform

/// A way to subscribe to external sources of events.
[<Struct>]
type Sub<'msg> = internal Sub of List<Sub.Subscriptions<'msg>>

[<RequireQualifiedAccess>]
module Sub =

  /// Available types of subscriptions.
  /// Only useful for tests.
  [<Struct>]
  type Subscriptions<'msg> = EachTick of Eff.EachTick.MySub<'msg>

  /// Tell the runtime that there are no subscriptions.
  val empty: Sub<'msg>

  /// When you need to subscribe to multiple things, you can create a batch of subscriptions.
  val concat: subs: List<Sub<'msg>> -> Sub<'msg>

  /// Transform the messages produced by a subscription.
  /// This is very rarely useful in well-structured code.
  val map: f: ('t -> 'msg) -> subs: Sub<'t> -> Sub<'msg>

  val internal singleton: value: Subscriptions<'msg> -> Sub<'msg>

  val internal unsafePerform:
    effects: Effects<'msg> -> _dispatch: ('msg -> unit) -> subs: List<Subscriptions<'msg>> -> unit

  val internal unwrap: sub: Sub<'msg> -> List<Sub.Subscriptions<'msg>>
