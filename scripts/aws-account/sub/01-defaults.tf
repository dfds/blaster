variable "aws_default_region" {
  default = "eu-central-1"
}

variable "cloudtrail_bucket" {
  default = "dfds-audit"
}

variable "admin_arn" {
  default = "arn:aws:iam::aws:policy/AdministratorAccess"
}

variable "viewonly_arn" {
  default = "arn:aws:iam::aws:policy/job-function/ViewOnlyAccess"
}

variable "cloudwatch_read_arn" {
  default = "arn:aws:iam::aws:policy/CloudWatchLogsReadOnlyAccess"
}

variable "lambda_read_arn" {
  default = "arn:aws:iam::aws:policy/AWSLambdaReadOnlyAccess"
}

variable "developer_default_arn" {
  default = "arn:aws:iam::aws:policy/PowerUserAccess"
}

variable "billing_default_arn" {
  default = "arn:aws:iam::aws:policy/job-function/Billing"
}
