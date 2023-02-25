// @after ./TryIO.fsi

namespace rec Tea.Platform

[<Struct>]
type TryIO<'a, 'x> =
  internal
  | Pure of value: Result<'a, 'x>
  | Perf of effect: (unit -> Result<'a, 'x>)
  | Bind of chain: (unit -> TryIO<'a, 'x>)

[<RequireQualifiedAccess>]
module TryIO =

  let from value = Pure(Ok value)

  let bind f tryIO =
    match tryIO with
    | Pure(Ok value) -> f value

    | Pure(Error error) -> Pure <| Error error

    | Perf unsafePerform ->
      Bind(fun () ->
        match unsafePerform () with
        | Ok value -> f value
        | Error error -> Pure <| Error error
      )

    | Bind continuation -> Bind(continuation >> bind f)

  let map (f: 'a -> 'b) (tryIO: TryIO<'a, 'x>) : TryIO<'b, 'x> =
    match tryIO with
    | Pure(Ok value) -> Pure <| (Ok <| f value)

    | Pure(Error error) -> Pure <| Error error

    | Perf unsafePerform ->
      Bind(fun () ->
        match unsafePerform () with
        | Ok value -> Pure <| (Ok <| f value)
        | Error error -> Pure <| Error error
      )

    | Bind continuation -> Bind(continuation >> map f)

  let perform tryIO =
    match tryIO with
    | Pure value -> value

    | Perf unsafePerform -> unsafePerform ()

    | Bind continuation -> perform <| continuation ()
