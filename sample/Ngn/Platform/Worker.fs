namespace rec Ngn.Platform

module internal Worker =

  open Tea.Platform

  type Program<'model, 'msg> =
    Worker.Program<'model, 'msg, List<Cmd.Commands<'msg>>, List<Sub.Subscriptions<'msg>>>
