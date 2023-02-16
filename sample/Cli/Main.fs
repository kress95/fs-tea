namespace rec Cli

module Main =

  open Ngn.Platform

  [<EntryPoint>]
  let main args =
    let flags: App.Flags = { None = () }

    let mutable runtime = Game.init App.Main.application flags

    Game.run runtime

    0
