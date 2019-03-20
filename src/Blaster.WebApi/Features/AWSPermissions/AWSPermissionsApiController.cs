using System.Threading.Tasks;
using Blaster.WebApi.Features.AWSPermissions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blaster.WebApi.Features.AWSPermissions
{
    [Route("api/awspermissions")]
    [ApiController]
    public class AWSPermissionsApiController : ControllerBase
    {
        [HttpGet("{key}", Name = "GetAllAWSPermissionsByKey")]
        public async Task<ActionResult<AWSPermissionsListResponse>> GetAllAWSPermissionsByKey(string key)
        {
            if (key == "Fo")
            {
                return BadRequest();
            }
            var list = new AWSPermissionsListResponse()
            {
                Items = new[]
                {
                    new AWSPermissionDTO()
                    {
                        PolicyName = "foo1-prefixed-by-capability-v001",
                        PolicyDocument = "{\r\n \"Version\": \"2012-10-17\",\r\n    \"Statement\": [\r\n        {\r\n            \"Effect\": \"Allow\",\r\n            \"Action\": [\r\n                \"dynamodb:DescribeReservedCapacityOfferings\",\r\n                \"dynamodb:ListGlobalTables\",\r\n                \"dynamodb:TagResource\",\r\n                \"dynamodb:UntagResource\",\r\n                \"dynamodb:ListTables\",\r\n                \"dynamodb:DescribeReservedCapacity\",\r\n                \"dynamodb:ListBackups\",\r\n                \"dynamodb:PurchaseReservedCapacityOfferings\",\r\n                \"dynamodb:ListTagsOfResource\",\r\n                \"dynamodb:DescribeTimeToLive\",\r\n                \"dynamodb:DescribeLimits\",\r\n                \"dynamodb:ListStreams\"\r\n            ],\r\n            \"Resource\": \"*\"\r\n        },\r\n        {\r\n            \"Effect\": \"Allow\",\r\n            \"Action\": \"dynamodb:*\",\r\n            \"Resource\": [\r\n                \"arn:aws:dynamodb:::table/smartdata3*/backup/\",\r\n                \"arn:aws:dynamodb:::table/smartdata3*/stream/\",\r\n                \"arn:aws:dynamodb:::table/smartdata3*/index/\",\r\n                \"arn:aws:dynamodb:::global-table/capacityName*\"\r\n            ]\r\n        },\r\n        {\r\n            \"Effect\": \"Allow\",\r\n            \"Action\": \"dynamodb:*\",\r\n            \"Resource\": \"arn:aws:dynamodb:::table/smartdata3*\"\r\n        }\r\n    ]\r\n}"
                    },
                    new AWSPermissionDTO()
                    {
                        PolicyName = "bar1-prefixed-by-capability-v002",
                        PolicyDocument = "{\r\n    \"Version\": \"2012-10-17\",\r\n    \"Statement\": [\r\n        {\r\n            \"Effect\": \"Allow\",\r\n            \"Action\": \"s3:ListAllMyBuckets\",\r\n            \"Resource\": \"*\"\r\n        },\r\n        {\r\n            \"Effect\": \"Allow\",\r\n            \"Action\": \"s3:*\",\r\n            \"Resource\": \"arn:aws:s3:::smartdata3-*\"\r\n        }\r\n    ]\r\n}"
                    }
                }
            };

            return list;
        }
    }
}