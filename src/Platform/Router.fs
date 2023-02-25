// @after ./IO.fs
// @after ./Router.fsi

namespace rec Tea.Platform

[<Struct>]
type Router<'appMsg, 'selfMsg> = internal {
  Dispatch: 'appMsg -> unit
  Loopback: 'selfMsg -> unit
}

module Router =

  let dispatch router msg = router.Dispatch msg

  let loopback router msg = router.Loopback msg

  let sendToApp router msg = Perf(fun () -> dispatch router msg)

  let sendToSelf router msg = Perf(fun () -> loopback router msg)
