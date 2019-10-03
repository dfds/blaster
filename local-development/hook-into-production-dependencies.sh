#!/bin/bash

kubectl -n selfservice port-forward svc/capability-service 50900:80 &
kubectl -n selfservice port-forward svc/aws-janitor 50901:80 &
kubectl -n selfservice-herald-vbqkr port-forward svc/harald 5123:80
