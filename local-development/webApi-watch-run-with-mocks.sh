#!/bin/bash

(cd ../src/Blaster.WebApi/ &&  BLASTER_CAPABILITYSERVICE_API_URL=http://localhost:50900 BLASTER_IAMROLESERVICE_API_URL=http://localhost:50901 dotnet watch run)