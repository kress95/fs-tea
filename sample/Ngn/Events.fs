//- require "./Platform/Sub.fs"
//- require "./Events.fsi"

module rec Ngn.Events

open Ngn.Platform

let eachTick toMsg = Sub.singleton <| Sub.EachTick toMsg
