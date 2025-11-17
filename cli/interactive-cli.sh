#!/usr/bin/env sh

read -p "Website?        " web
read -p "Secret input?   " inp
echo ""
./watercli.exe ${web} ${inp}
read -p "Press the enter key to close." fin
