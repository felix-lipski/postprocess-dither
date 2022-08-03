{
  inputs = {
    nixpkgs.url = github:NixOS/nixpkgs/nixos-unstable;
    flake-utils.url = github:numtide/flake-utils;
  };

  outputs = { nixpkgs, self, flake-utils, ... }@inputs:
    flake-utils.lib.eachDefaultSystem
      (system:
      let 
        pkgs = import nixpkgs {
          config = { allowUnfree = true; };
          inherit system;
        };
      in {
        devShell =
          pkgs.mkShell {
            buildInputs = with pkgs; [ 
              raylib
              libGL xorg.libX11
            ]; 
            shellHook = ''
              '' ;
          };
        }
      );
}
