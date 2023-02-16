{ pkgs ? import <nixpkgs> { } }:
with pkgs;
mkShell {
  buildInputs = with pkgs; [
    dotnet-sdk_7
  ];
  shellHook = ''
    export DOTNET_ROOT="${pkgs.dotnet-sdk_7}";
  '';
}
