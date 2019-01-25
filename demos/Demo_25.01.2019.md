# DED - Dev demo 25th January 2019
## Draw service context (Jens)
* Including:
  * ALB
  * Azure AD
  * auth-proxy
  * Blaster
  * team-service
  * iam-roleservice
  * rolemapperservice
## Create/join team (Jens)
* Show UI.

## Show changes in IAM (Jakob)
* Visit https://console.aws.amazon.com/iam/home?#/roles
* Search for team created.

## Show changes to config map (Jakob)
* Visit https://console.aws.amazon.com/s3/object/configmapbucket/ConfigMap.yml?region=eu-central-1&tab=overview
* Open file and show added role mapping.
  * If it won't open by choosing "Open", download the file and show it.

## Show changes to AD (Thomas)
* Team (AD-group created).
  * Member who have joined the team.

## Usage flow/setup (Thomas)
### Pre-requisites (kubectl, aws-cli, aws-iam-authenticator, saml2aws).
* See OneNote: https://dfds.sharepoint.com/sites/msteams_7d4df0/_layouts/15/WopiFrame.aspx?sourcedoc={df87278b-2824-4730-adaf-3844624c1c29}&action=edit&wd=target%28Self-service.one%7C6d77ebaf-18ce-4bd8-85c2-2eb411f5f2bf%2FCLI%20setup%20saml2aws%2C%20kubectl%2C%20aws%7Cc641c249-a5f3-4a1f-8d44-6c5d1da703ea%2F%29&wdorigin=703

### Download Kubernetes config
* From LastPass: https://lastpass.com
* Secret note "k8s-eks-saml2aws".
* Save to ~/.kube/config or %USERPROFILE%/.kube/config

### Saml2Aws login
* Use "saml" AWS profile:
  * Linux/Mac: *export AWS_PROFILE=saml *
  * Windows: *setx AWS_PROFILE "saml" -m*
* ***saml2aws login***
* Assume team just created.

### Invoke kubectl commands
* Eg. kubectl get ns, get pods, etc.
* Demonstrate the user is only allowed to read, eg. not delete.
  * Try to delete an old replicaset.

## Leave team (Jens or Thomas?)
