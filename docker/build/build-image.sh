#!/bin/bash
rm -Rf build/
mkdir build
cp -R ../../backend/* build

docker build -t os2services:rolesfromad .

rm -Rf build/
