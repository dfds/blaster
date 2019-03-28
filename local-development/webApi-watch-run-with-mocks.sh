#!/bin/bash

BLASTER_CAPABILITYSERVICE_API_URL=http://localhost:50900 \
BLASTER_AWS_JANITOR_API_URL=http://localhost:50901 \
dotnet watch --project ../src/Blaster.WebApi/ run