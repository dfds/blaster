[![Build Status](https://dfds.visualstudio.com/DevelopmentExcellence/_apis/build/status/Blaster-CI?branch=master)](https://dfds.visualstudio.com/DevelopmentExcellence/_build/latest?definitionId=803&branch=master)[![Release Status](https://dfds.vsrm.visualstudio.com/_apis/public/Release/badge/ace5e409-c242-4356-93f4-23c53a3dc87b/14/18)](https://dfds.visualstudio.com/DevelopmentExcellence/_build/latest?definitionId=803&branch=master)

# blaster
Automation platform for http://kubernetes.io/ running on AWS with Active Directory Federation Services (AD FS) from Microsoft.

> Autobot Blaster's speciality is communications. He possesses a Flight Pack that can transform into a signal-jamming, electro-scrambler gun.

DFDS-Blaster is a front-end/facade service that requires other services to do actual work. Some notable functionality includes

- Mapping AD users and groups to IAM roles
  - Which in turn can allow a user with the requisite AD credentials to safely access resources in Kubernetes
- Setting up container repositories in ECR with appropriate naming rules and permissions applied
- Creating Kubernetes namespaces with, again, appropriate naming rules and permissions applied
- Storing the mappings and permissions durably so that they can be replayed for disaster recovery/2nd site setup etc

Currently, the "Capability" is the root entity in Blaster. 

```ascii
                   +-------------+
                   |             | 1      * +------------+          +----------+
                   | Capability  +----------+  Member    +---------->          |
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

Then read pipeline.sh.

Then the steps are:
```
# OPEN THREE (3) TERMINAL WINDOWS FOR THIS:

# 1: dotnet BFF
cd src
npm install
cd Blaster.WebApi
dotnet restore
dotnet watch run

# 2: webpack frontend
cd src/Blaster.WebApi
npm start # runs webpack

# 3: dependency fakes
cd fake_dependencies
docker-compose up --build
```

The above setup should run with no further changes. If you need a real backend, go look at e.g.  https://github.com/dfds/capability-service or some of the others.

A number of environment variables are read to point to the relevant services but out of the box, everything points to localhost so if other services are running on the same machine, Blaster should be good to go.

## Structure

    ./fake_dependencies: Fake dependencies which can be spun up to ease local development.
    ./k8s: Kubernetes tokenized manifests.
    ./src: Blaster source.
        ./auth-proxy: NodeJS proxy sitting between AWS ALB and Blaster to decouple Blaster from authentication implementation.
        ./Blaster.Tests: Xunit based tests.
        ./Blaster.WebApi: .NET Core WebApi code and frontend.

Blaster uses Vue for client-side framework.

## Call flow (prod)
While in development, the actual user is magistubbed. In production, the auth-proxy sits in front and handles the workload. There is a sample docker-compose definition is available in `fake_dependencies/docker-compose-with-auth-proxy.yml`. Note that this is probably not ready to run. 

The call flow from user to Blaster goes through an AWS ALB for authentication. To simulate the JWT from the ALB (1) below is replaced with a header-injector proxy.
 
(2) is replaced with a fake capability service.

```ascii
                  +---------+
                  | (1)     |
+------------+    |         |      +-----+------+     +-----+-------+     +-----+--------+
|            |    |         |      |            |     |             |     | (2)          |
|    User    +--->+ AWS ALB +----->+ auth-proxy +---->+   Blaster   +---->+  Capability  |
|            |    |         |      |            |     |             |     |   Service    |
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


## Deployment prerequisites

With the change to scoped Kubernetes service connections during deploment, certain manifests have been moved out of the *k8s* directory and moved to the *k8s_initial* directory.

The manifests within *k8s_initial* will have to be run manually or with a different service connection due to elevated rights.

---

If the scoped service account is missing for deployment, see https://wiki.dfds.cloud/en/teams/devex/selfservice/Kubernetes-selfservice-deployment-setup
