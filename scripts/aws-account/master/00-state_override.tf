terraform {
  backend "s3" {
    bucket = "dfdsstate"
    region = "eu-central-1"
  }
}
