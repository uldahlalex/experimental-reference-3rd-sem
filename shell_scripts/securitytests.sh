#!/usr/bin/env bash

for file in ../tests/security/*.json
do
  newman run "$file" --insecure
done