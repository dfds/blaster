#!/bin/bash

(cd ../src/Blaster.WebApi/ && npm install &)

(cd ../src/Blaster.WebApi/ && dotnet restore &)