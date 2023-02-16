//- require "./IO.fsi"

namespace rec Tea.Platform

[<Struct>]
type IO<'T> =
  internal
  | Pure of value: 'T
  | Perf of effect: (unit -> 'T)
  | Bind of chain: (unit -> IO<'T>)

[<RequireQualifiedAccess>]
module IO =

  let from value = Pure(value)

  let bind f io =
    match io with
    | Pure value -> f value

    | Perf unsafePerform -> Bind(unsafePerform >> f)

    | Bind continuation -> Bind(continuation >> bind f)

  let map f io =
    match io with
    | Pure value -> Pure(f value)

    | Perf unsafePerform -> Perf(unsafePerform >> f)

    | Bind continuation -> Bind(continuation >> map f)

  let perform io =
    match io with
    | Pure value -> value

    | Perf unsafePerform -> unsafePerform ()

    | Bind continuation -> perform <| continuation ()
