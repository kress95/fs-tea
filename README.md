This is a base for creating custom tailored _TEAs_.

> This is more of a example than anything else.

Check `sample/Ngn` to see a custom _TEA_ and `sample/App` to see the usage.

---

The names had to change for convenience:

- [Router = Platform.Router](https://package.elm-lang.org/packages/elm/core/latest/Platform#Router)
- [Effect = Effect Manager](https://simonh1000.github.io/2017/05/effect-managers/)
- [IO = Task Never](https://package.elm-lang.org/packages/elm/core/latest/Task#perform)
- [TryIO = Task](https://package.elm-lang.org/packages/elm/core/latest/Task#Task)
- [Reducer = Platform.worker](https://package.elm-lang.org/packages/elm/core/latest/Platform#worker) but without subscriptions 
- [Worker = Platform.worker](https://package.elm-lang.org/packages/elm/core/latest/Platform#worker)

Some internals such as [Process](https://package.elm-lang.org/packages/elm/core/latest/Process) weren't remade (yet?).

> NOTE: only ever use this as a learning tool, I'm not going to update this often.
