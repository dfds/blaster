namespace Blaster.WebApi.Features.Dashboards
{
    public interface IJsonSerializer
    {
        string Serialize(object instance);
        T Deserialize<T>(string text);
    }
}