//- require "./Eff/EachTick.fs"

namespace rec Ngn.Platform

type internal Effects<'msg> =
  {
    OnEachTick: List<Eff.EachTick.MySub<'msg>> -> unit
  }
