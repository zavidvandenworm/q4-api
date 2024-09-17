{ pkgs ? import <nixpkgs> {} }:

pkgs.mkShell {
  buildInputs = [
    pkgs.openssl
    pkgs.openssl_1_1   # For backward compatibility with older libraries
    pkgs.dotnet-sdk    # .NET SDK
  ];
}
