#!/bin/bash

kubectl -n selfservice port-forward svc/capability-service 50900:80 &
kubectl -n selfservice port-forward svc/k8s-janitor 50901:80 &
kubectl -n selfservice port-forward svc/aws-janitor 50902:80 &