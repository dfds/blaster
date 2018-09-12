variable "account_name" {
  type = "string"
}

provider "aws" {
  alias = "dfds"
}

variable "aws_org_rolename" {
  default = "OrgRole"
}

resource "aws_organizations_account" "dfds" {
  name                       = "dfds-${var.account_name}"
  email                      = "aws.${replace(var.account_name, "-", ".")}@dfds.com"
  iam_user_access_to_billing = "ALLOW"
  role_name                  = "${var.aws_org_rolename}"
}

output "account_id" {
  value = "${aws_organizations_account.dfds.id}"
}

output "email" {
  value = "${aws_organizations_account.dfds.email}"
}
