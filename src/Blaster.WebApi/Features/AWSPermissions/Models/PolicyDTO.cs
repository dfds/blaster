namespace Blaster.WebApi.Features.AWSPermissions.Models
{
    public class PolicyDTO
    {
        public PolicyDTO(string policyName, object policyDocument)
        {
            PolicyName = policyName;
            PolicyDocument = policyDocument;
        }
        
        public string PolicyName { get; }
        public object PolicyDocument { get; }
    }
}