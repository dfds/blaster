[![Build Status](https://dfds.visualstudio.com/DevelopmentExcellence/_apis/build/status/Blaster-CI?branch=master)](https://dfds.visualstudio.com/DevelopmentExcellence/_build/latest?definitionId=803&branch=master)[![Release Status](https://dfds.vsrm.visualstudio.com/_apis/public/Release/badge/ace5e409-c242-4356-93f4-23c53a3dc87b/14/18)](https://dfds.visualstudio.com/DevelopmentExcellence/_build/latest?definitionId=803&branch=master)

# blaster
Automation platform for http://kubernetes.io/ running on AWS with AFDS from Microsoft.

> Autobot Blaster's speciality is communications. He possesses a Flight Pack that can transform into a signal-jamming, electro-scrambler gun.

DFDS-Blaster is a front-end/facade service that requires other services to do actual work. Some notable functionality includes

- Mapping AD users and groups to IAM roles
  - Which in turn can allow a user with the requisite AD credentials to safely access resources in Kubernetes
- Setting up container repositories in ECR with appropriate naming rules and permissions applied
- Creating Kubernetes namespaces with, again, appropriate naming rules and permissions applied
- Storing the mappings and permissions durably so that they can be replayed for disaster recovery/2nd site setup etc

Currently, the "team" is the root entity in Blaster. 

```ascii
                   +-------------+
                   |             | 1      * +------------+          +----------+
                   |    Team     +----------+  Member    +---------->          |
              +---->             |          +------------+          |[Azure AD]|
              |    +-------+-----+                                  |          |
              |            ^                                        |          |
              |            |                                        |          |
   +----------+-+    +-----+-------+                                +----------+
   | Container  |    | Kubernetes  |
   | Repository |    | Namespace   |
   |            |    |             |
   +--+---------+    +-------------+
      |                 | 
      |                 |
      |                 |
+----------->Exported to|AWS/other...<---------------------+
      |                 |
      |                 |
      |                 |
   +--v-----+       +---v------+
   |        |       |          |
   |  ECR   |       |  EKS     |
   |        |       |          |
   +--------+       +----------+
```

# Getting started
Prerequisites:

0. (For Windows) Admin account on the local machine
1. Git
2. NodeJS/npm or yarn
5. dotnet core 2.1
6. You may also quickly want docker, docker-compose, awscli and kubectl but they are not prerequisites per-se

Then read pipeline.sh, and mostly ignore it. Once that's done, go:

```
cd src
npm install
dotnet restore Blaster.sln
dotnet watch run # or just run
```

Quite likely, nothing will work without at least one supporting service so go and grab https://github.com/dfds/team-service.

A number of environment variables are read to point to the relevant services but out of the box, everything points to localhost so if other services are running on the same machine, Blaster should be good to go.

## Structure
Currently, blaster is just a web UI. The planned CLI is not there yet.

    ./docker-compose.yml: Docker-compose definition to spin up fake dependencies for local development.
    ./fake_dependencies: Fake dependencies which can be spun up to ease local development.
    ./k8s: Kubernetes tokenized manifests.
    ./src: Blaster source.
        ./auth-proxy: NodeJS proxy sitting between AWS ALB and Blaster to decouple Blaster from authentication implementation.
        ./Blaster.Tests: Xunit based tests.
        ./Blaster.WebApi: .NET Core WebApi code and frontend.

Blaster uses Vue for client-side framework.

## Local development with fake dependencies
A Docker-compose definition is available in the root of the repository. This spins up fake dependencies to ease local development.

To spin up the fake dependencies run:

On first run:
```
docker-compose up --build
```
Otherwise:
```
docker-compose up
```
The call flow from user to Blaster goes through an AWS ALB for authentication. To simulate the JWT from the ALB (1) below is replaced with a header-injector proxy.
 
(2) is replaced with a fake team service.

```ascii
                  +---------+
                  | (1)     |
+------------+    |         |      +-----+------+     +-----+-------+     +-----+--------+
|            |    |         |      |            |     |             |     | (2)          |
|    User    +--->+ AWS ALB +----->+ auth-proxy +---->+   Blaster   +---->+ Team Service |
|            |    |         |      |            |     |             |     |              |
+------------+    |         |      +------------+     +-------------+     +--------------+
                  |         |
                  +----+----+
                       |
                       v
                +------+------+
                |             |
                |  Azure AD   +
                |             |
                +-------------+
```

# Deployment
FIXME This section and any notes on this are woefully out of date FIXME


# Pull request and dev stuff
FIXME This section and any notes on this are woefully out of date FIXME
