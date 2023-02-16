namespace rec Ngn.Platform

/// A way to perform side-effects.
[<Struct>]
type Cmd<'msg> = internal Cmd of List<Cmd.Commands<'msg>>

[<RequireQualifiedAccess>]
module Cmd =

  /// Available types of commands.
  /// Only useful for tests.
  [<Struct>]
  type Commands<'msg> = Loopback of 'msg

  /// Tell the runtime that there are no commands.
  val empty: Cmd<'msg>

  /// When you need the runtime system to perform a couple commands, you can batch them together.
  val concat: cmds: List<Cmd<'msg>> -> Cmd<'msg>

  /// Transform the messages produced by a command.
  /// This is very rarely useful in well-structured code.
  val map: f: ('a -> 'msg) -> cmd: Cmd<'a> -> Cmd<'msg>

  val internal singleton: value: Commands<'msg> -> Cmd<'msg>

  val internal unsafePerform: _effects: Effects<'msg> -> dispatch: ('msg -> unit) -> cmds: List<Commands<'msg>> -> unit

  val internal unwrap: cmd: Cmd<'msg> -> List<Cmd.Commands<'msg>>
