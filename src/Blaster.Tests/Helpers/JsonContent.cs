using System.Net.Http;
using System.Text;
using Blaster.WebApi.Features.Capabilities;

namespace Blaster.Tests.Helpers
{
    public class JsonContent : StringContent
    {
        public JsonContent(object instance)
            : base(ConvertToJson(instance), Encoding.UTF8, "application/json")
        {
        }

        public static string ConvertToJson(object instance)
        {
            if (instance == null)
            {
                return "{ }";
            }

            var serializer = new JsonSerializer();
            return serializer.Serialize(instance);
        }

        public static JsonContent Empty => new JsonContent(null);
    }
}