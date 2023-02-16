namespace rec Tea.Platform

/// TryIO describe operations that may fail, like HTTP requests.
[<Struct>]
type TryIO<'a, 'x> =
  internal
  | Pure of value: Result<'a, 'x>
  | Perf of effect: (unit -> Result<'a, 'x>)
  | Bind of chain: (unit -> TryIO<'a, 'x>)

[<RequireQualifiedAccess>]
module TryIO =

  /// Create container from a successful value.
  val from: value: 'a -> TryIO<'a, 'x>

  /// Use value yielded by the contained operation in another operation.
  val bind: f: ('a -> TryIO<'b, 'x>) -> tryIO: TryIO<'a, 'x> -> TryIO<'b, 'x>

  /// Transforms value yielded by the operation.
  val map: f: ('a -> 'b) -> tryIO: TryIO<'a, 'x> -> TryIO<'b, 'x>

  /// Performs contained operation yielding a result.
  val perform: tryIO: TryIO<'a, 'x> -> Result<'a, 'x>
