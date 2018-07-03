# Scripted AWS account provisioning and configuration

## Summary

Creates sub account through organisation master account. Then assumes the organisation role in the new account and configure properties (SAML, adds IAM roles, CloudTrail etc.).

You must manually configure tax settings as follows:

| Property | Value |
| --- | --- |
| Country | Denmark |
| Tax registration number | DK14194711 |
| Business Legal Name | DFDS A/S |

## Inspiration

Partially inspired by Emii Khaos' articles:

* https://medium.com/@EmiiKhaos/automated-aws-account-initialization-with-terraform-and-onelogin-saml-sso-1301ff4851ab
* https://medium.com/@EmiiKhaos/part-2-automated-aws-multi-account-setup-with-terraform-and-onelogin-sso-44baaf563877

## Prerequisites

* AWS CLI
* Terraform
* Access key and secret for user in master account ("terraform-account"), with the following policy attached ("TerraformCreateAccount"):

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": "s3:ListBucket",
            "Resource": "arn:aws:s3:::dfdsstate"
        },
        {
            "Effect": "Allow",
            "Action": [
                "s3:GetObject",
                "s3:PutObject"
            ],
            "Resource": "arn:aws:s3:::dfdsstate/accounts/*.tfstate"
        },
        {
            "Effect": "Allow",
            "Action": "organizations:*",
            "Resource": "*"
        }
    ]
}
```

## Example usage

```shell
export AWS_ACCESS_KEY_ID = "AWS_KEY_ID_FOR_MASTER_SERVICE_ACCOUNT"
export AWS_SECRET_ACCESS_KEY = "AWS_SECRET_ACCESS_KEY_FOR_MASTER_SERVICE_ACCOUNT"
./account.sh plan PassengerBooking NonProd
```

## To do

* Exception handling at crucial points
* Poll of created account is ready, instead of stupid wait
* Add DFDS custom ReadOnly policy, attach to ADFS-ReadOnly role
* Add DFDS custom DevOps policy, attach to ADFS-DevOps role
* Different default policies for Developer role, based on Prod/NonProd
* Configure budget - set to?
* Configure tax settings

## Potential improvements

* Retain local state files (two for each account) for faster Terraform init
* Assign teams access to ADFS-Admin role, or ADFS-Developer role with admin policy, in non-prod accounts?