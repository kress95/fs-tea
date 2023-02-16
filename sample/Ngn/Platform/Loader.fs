namespace Ngn.Platform

open System
open System.IO
open System.Runtime.InteropServices

[<RequireQualifiedAccess>]
module Loader =

  [<AutoOpen>]
  module private Private =

    [<DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)>]
    [<return: MarshalAs(UnmanagedType.Bool)>]
    extern bool SetDefaultDllDirectories(int directoryFlags)

    [<DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)>]
    extern void AddDllDirectory(string lpPathName)

    [<DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)>]
    [<return: MarshalAs(UnmanagedType.Bool)>]
    extern bool SetDllDirectory(string lpPathName)

    let LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000

  let unsafeLoad () =
    if Environment.OSVersion.Platform = PlatformID.Win32NT then
      let path =
        Path.Combine(
          AppDomain.CurrentDomain.BaseDirectory,
          (if Environment.Is64BitProcess then "x64" else "x86")
        )

      try
        ignore <| SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS)
        AddDllDirectory(path)
      with _ ->
        ignore <| SetDllDirectory(path)
        ()
