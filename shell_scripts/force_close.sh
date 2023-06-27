#!/usr/bin/env bash
#force closes program running on port 5000
sudo kill -9 $(sudo lsof -t -i:5000)