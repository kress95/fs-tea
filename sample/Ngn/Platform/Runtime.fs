//- require "./Cmd.fs"
//- require "./Eff/EachTick.fs"
//- require "./Sub.fs"
//- require "./Worker.fs"

namespace rec Ngn.Platform

open Tea.Platform
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type internal Programs<'model, 'msg> =
  {
    App: Worker.Program<'model, 'msg>
    EachTick: Eff.EachTick.Program<'msg>
  }

type internal Runtime<'model, 'msg>(p: Programs<'model, 'msg>) as self =
  inherit Game()
  do self.Content.RootDirectory <- "Content"

  let toApp = Worker.send p.App
  let graphics = new GraphicsDeviceManager(self)
  let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

  member this.programs = p

  override this.Initialize() =
    spriteBatch <- new SpriteBatch(this.GraphicsDevice)
    base.Initialize()

    ()

  override this.LoadContent() = ()

  override this.Update(gameTime) =
    Effect.send p.EachTick gameTime
    Effect.update p.EachTick
    Effect.pipe p.EachTick toApp
    Worker.update p.App

    ()

  override this.Draw(gameTime) =
    this.GraphicsDevice.Clear Color.CornflowerBlue
    ()
