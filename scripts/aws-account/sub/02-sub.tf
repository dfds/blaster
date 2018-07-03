variable "account_name" {
  type = "string"
}

variable "account_id" {
  type = "string"
}

provider "aws" {
  alias = "dfds"
}

// Set acccount alias
resource "aws_iam_account_alias" "dfds" {
  account_alias = "dfds-${var.account_name}"
  provider      = "aws.dfds"
}

// Add SAML provider
resource "aws_iam_saml_provider" "dfds_adfs" {
  name                   = "ADFS"
  saml_metadata_document = "${file("../FederationMetadata.xml")}"
  provider               = "aws.dfds"
}

// Asume role with SAML policy document
data "aws_iam_policy_document" "dfds_adfs_assume" {
  statement {
    sid     = "ADFS"
    actions = ["sts:AssumeRoleWithSAML"]

    principals {
      type        = "Federated"
      identifiers = ["${aws_iam_saml_provider.dfds_adfs.arn}"]
    }

    condition {
      test     = "StringEquals"
      variable = "SAML:aud"
      values   = ["https://signin.aws.amazon.com/saml"]
    }
  }

  provider = "aws.dfds"
}

resource "aws_cloudtrail" "audit" {
  name                          = "audit"
  s3_bucket_name                = "${var.cloudtrail_bucket}"
  s3_key_prefix                 = "dfds-${var.account_name}"
  include_global_service_events = true
  is_multi_region_trail         = true
  enable_log_file_validation    = true
}

// --------------------------------------------------
// Create IAM roles
// --------------------------------------------------

// Create ADFS-Admin role
resource "aws_iam_role" "dfds_admin" {
  name               = "ADFS-Admin"
  assume_role_policy = "${data.aws_iam_policy_document.dfds_adfs_assume.json}"
  provider           = "aws.dfds"
}

resource "aws_iam_role_policy_attachment" "dfds_admin" {
  role       = "${aws_iam_role.dfds_admin.name}"
  policy_arn = "${var.administrator_default_arn}"
  provider   = "aws.dfds"
}

// Developer, DevOps, ReadOnly

