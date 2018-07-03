variable "aws_default_region" {
  default = "eu-central-1"
}

variable "cloudtrail_bucket" {
  default = "dfds-audit"
}

variable "administrator_default_arn" {
  default = "arn:aws:iam::aws:policy/AdministratorAccess"
}

variable "developer_default_arn" {
  default = "arn:aws:iam::aws:policy/PowerUserAccess"
}

variable "billing_default_arn" {
  default = "arn:aws:iam::aws:policy/job-function/Billing"
}
