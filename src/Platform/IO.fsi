namespace rec Tea.Platform

/// IO describe operations that cannot fail, like dispatching a message to local queue.
[<Struct>]
type IO<'a> =
  internal
  | Pure of value: 'a
  | Perf of effect: (unit -> 'a)
  | Bind of chain: (unit -> IO<'a>)

[<RequireQualifiedAccess>]
module IO =

  /// Create container from a successful value.
  val from: value: 'a -> IO<'a>

  /// Use value yielded by the contained operation in another operation.
  val bind: f: ('a -> IO<'b>) -> io: IO<'a> -> IO<'b>

  /// Transforms value yielded by the operation.
  val map: f: ('a -> 'b) -> io: IO<'a> -> IO<'b>

  /// Performs contained operation yielding a result.
  val perform: io: IO<'a> -> 'a
