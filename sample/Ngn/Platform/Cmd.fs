//- require "./Effects.fs"
//- require "./Cmd.fsi"

namespace rec Ngn.Platform

open Tea.Platform

[<Struct>]
type Cmd<'msg> = internal | Cmd of List<Cmd.Commands<'msg>>

[<RequireQualifiedAccess>]
module Cmd =

  [<Struct>]
  type Commands<'msg> = | Loopback of 'msg

  let empty = Cmd List.empty

  let concat cmds =
    Cmd <| List.concat (List.map unwrap cmds)

  let map f (Cmd cmd) = Cmd <| List.map (mapEff f) cmd

  let internal singleton value = Cmd <| List.singleton value

  let internal unsafePerform
    (_effects: Effects<'msg>)
    (dispatch: 'msg -> unit)
    (cmds: List<Commands<'msg>>)
    : unit
    =
    for cmd in cmds do
      match cmd with
      | Loopback msg -> dispatch msg

  let internal unwrap (Cmd commands) = commands

  let private mapEff f eff =
    match eff with
    | Loopback msg -> Loopback(f msg)
