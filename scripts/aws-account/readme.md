Largely based on Emii Khaos' articles:

* https://medium.com/@EmiiKhaos/automated-aws-account-initialization-with-terraform-and-onelogin-saml-sso-1301ff4851ab
* https://medium.com/@EmiiKhaos/part-2-automated-aws-multi-account-setup-with-terraform-and-onelogin-sso-44baaf563877


```PowerShell
$env:AWS_ACCESS_KEY_ID = "YOUR_AWS_ACCESS_KEY_ID"
$env:AWS_SECRET_ACCESS_KEY = "YOUR_AWS_SECRET_ACCESS_KEY"
$env:AWS_DEFAULT_REGION = "eu-central-1"
```

aws s3 mb s3://dfdsstate --region eu-central-1
aws s3api put-bucket-versioning --bucket dfdsstate --versioning-configuration Status=Enabled