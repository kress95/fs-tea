module rec Ngn.Events

open Ngn.Platform
open Microsoft.Xna.Framework

/// Subscribe to each tick of the game
val eachTick: toMsg: (GameTime -> 'Msg) -> Sub<'Msg>
