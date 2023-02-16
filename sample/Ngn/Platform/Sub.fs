//- require "./Eff/EachTick.fs"
//- require "./Effects.fs"
//- require "./Sub.fsi"

namespace rec Ngn.Platform

/// A way to observe side-effects.
[<Struct>]
type Sub<'msg> = internal | Sub of List<Sub.Subscriptions<'msg>>

[<RequireQualifiedAccess>]
module Sub =

  /// Available types of subscriptions.
  /// Only useful for tests.
  [<Struct>]
  type Subscriptions<'msg> = | EachTick of Eff.EachTick.MySub<'msg>

  let empty = Sub List.empty

  let concat subs =
    Sub <| List.concat (List.map unwrap subs)

  let map f (Sub subs) = Sub <| List.map (mapEff f) subs

  let internal singleton value = Sub <| List.singleton value

  let internal unsafePerform
    (effects: Effects<'msg>)
    (_dispatch: ('msg -> unit))
    (subs: List<Subscriptions<'msg>>)
    : unit
    =
    let mutable eachTick = List.empty

    for sub in subs do
      match sub with
      | EachTick sub -> eachTick <- sub :: eachTick

    effects.OnEachTick eachTick

  let internal unwrap (Sub subs) = subs

  let private mapEff f eff =
    match eff with
    | EachTick msg -> EachTick(msg >> f)
