{ pkgs ? import <nixpkgs> { } }:

with pkgs;
[ raylib libGL xorg.libX11 ]
