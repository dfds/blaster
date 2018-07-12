# Verify arguments
if [[ "$#" -lt 3 ]]; then
    echo "Syntax:  $0 <TerraformVerb> <ProjectName> <Environment> (-auto-approve)"
    echo "Example: $0 plan Shared NonProd"
    echo "         $0 apply Shared NonProd -auto-approve"
    exit 1
fi


# Define variables
. constants.sh
export AWS_DEFAULT_REGION="eu-central-1"
TERRAFORM_VERB=$1
PROJECT_NAME=$2
ENVIRONMENT=$3
AWS_ACCOUNT_NAME="${PROJECT_NAME,,}-${ENVIRONMENT,,}"
AWS_MASTER_S3_BUCKET="dfdsstate"
AWS_ROLE_SESSION="TerraformOrgRole"
AWS_SUB_S3_BUCKET="dfds${AWS_ACCOUNT_NAME//-}state"


# echo -e "\n\n${out_lyellow}Create S3 bucket for state files in the master account${out_reset}\n" # reference
# aws s3 mb s3://${AWS_MASTER_S3_BUCKET} --region eu-central-1
# aws s3api put-bucket-versioning --bucket ${AWS_MASTER_S3_BUCKET} --versioning-configuration Status=Enabled


echo -e "\n\n${out_lyellow}Create new sub account under master account${out_reset}\n"
pushd ./master > /dev/null
terraform init -backend-config "bucket=${AWS_MASTER_S3_BUCKET}" -backend-config "key=accounts/${AWS_ACCOUNT_NAME}.tfstate"
terraform ${TERRAFORM_VERB} -var "account_name=${AWS_ACCOUNT_NAME}" $4
AWS_ACCOUNT_ID=$(terraform output account_id)
rm -rf ./.terraform # remove local TerraForm state files
popd > /dev/null


echo -e "\n\n${out_lyellow}Waiting a bit for account to finish provisioning${out_reset}\n"
sleep 30


echo -e "\n\n${out_lyellow}Assume the OrgRole in the new sub account${out_reset}\n"
AWS_ASSUMED_CREDS=( $(aws sts assume-role \
    --role-arn "arn:aws:iam::${AWS_ACCOUNT_ID}:role/OrgRole" \
    --role-session-name ${AWS_ROLE_SESSION} \
    --query 'Credentials.[AccessKeyId,SecretAccessKey,SessionToken]' \
    --output text
) )
AWS_ASSUMED_ACCESS_KEY_ID=${AWS_ASSUMED_CREDS[0]}
AWS_ASSUMED_SECRET_ACCESS_KEY=${AWS_ASSUMED_CREDS[1]}
AWS_ASSUMED_SESSION_TOKEN=${AWS_ASSUMED_CREDS[2]}
echo -e "\n${out_lcyan}Access key ID for assumed role in ${AWS_ACCOUNT_ID} is: ${AWS_ASSUMED_ACCESS_KEY_ID}${out_reset}\n"


echo -e "\n\n${out_lyellow}Create S3 bucket for state files in the new sub account${out_reset}\n"
AWS_ACCESS_KEY_ID=${AWS_ASSUMED_ACCESS_KEY_ID} AWS_SECRET_ACCESS_KEY=${AWS_ASSUMED_SECRET_ACCESS_KEY} AWS_SESSION_TOKEN=${AWS_ASSUMED_SESSION_TOKEN} \
    aws s3 mb s3://${AWS_SUB_S3_BUCKET}
AWS_ACCESS_KEY_ID=${AWS_ASSUMED_ACCESS_KEY_ID} AWS_SECRET_ACCESS_KEY=${AWS_ASSUMED_SECRET_ACCESS_KEY} AWS_SESSION_TOKEN=${AWS_ASSUMED_SESSION_TOKEN} \
    aws s3api put-bucket-versioning --bucket ${AWS_SUB_S3_BUCKET} --versioning-configuration Status=Enabled


echo -e "\n\n${out_lyellow}Configure new sub account${out_reset}\n"
# Terraform currently uses key and secret directly, instead of assuming role itself. Not sure if desirable.
pushd ./sub > /dev/null
AWS_ACCESS_KEY_ID=${AWS_ASSUMED_ACCESS_KEY_ID} AWS_SECRET_ACCESS_KEY=${AWS_ASSUMED_SECRET_ACCESS_KEY} AWS_SESSION_TOKEN=${AWS_ASSUMED_SESSION_TOKEN} \
    terraform init -backend-config "bucket=${AWS_SUB_S3_BUCKET}" -backend-config "key=account/${AWS_ACCOUNT_NAME}.tfstate"
AWS_ACCESS_KEY_ID=${AWS_ASSUMED_ACCESS_KEY_ID} AWS_SECRET_ACCESS_KEY=${AWS_ASSUMED_SECRET_ACCESS_KEY} AWS_SESSION_TOKEN=${AWS_ASSUMED_SESSION_TOKEN} \
    terraform $TERRAFORM_VERB -var "account_name=${AWS_ACCOUNT_NAME}" -var "account_id=${AWS_ACCOUNT_ID}" $4
AWS_DEPLOY_KEY=$(AWS_ACCESS_KEY_ID=${AWS_ASSUMED_ACCESS_KEY_ID} AWS_SECRET_ACCESS_KEY=${AWS_ASSUMED_SECRET_ACCESS_KEY} AWS_SESSION_TOKEN=${AWS_ASSUMED_SESSION_TOKEN} \
    terraform output deploy_key)
AWS_DEPLOY_SECRET=$(AWS_ACCESS_KEY_ID=${AWS_ASSUMED_ACCESS_KEY_ID} AWS_SECRET_ACCESS_KEY=${AWS_ASSUMED_SECRET_ACCESS_KEY} AWS_SESSION_TOKEN=${AWS_ASSUMED_SESSION_TOKEN} \
    terraform output deploy_secret)
rm -rf ./.terraform # remove local TerraForm state files
popd > /dev/null


echo -e "\n\n${out_lred}You need to manually update tax settings under 'My Account'!\n
  Country:                  Denmark
  Tax registration number:  DK14194711
  Business Legal Name:      DFDS A/S
${out_reset}\n"


echo -e "\n\n${out_lcyan}The following service account has been created for deploying in ${PROJECT_NAME}-${ENVIRONMENT}:\n
  User:                     deploy
  Access key ID:            ${AWS_DEPLOY_KEY}
  Secret access key:        ${AWS_DEPLOY_SECRET}
${out_reset}\n"


echo -e "\n\n${out_lyellow}Now run the following PowerShell script to create required AD groups:${out_reset}\n"
echo ".\Create-ADGroups.ps1 -ProjectName ${PROJECT_NAME} -Environment ${ENVIRONMENT} -AWSAccountNo ${AWS_ACCOUNT_ID} (-TeamGroup 'Team AD Group')"
echo -e "\n"