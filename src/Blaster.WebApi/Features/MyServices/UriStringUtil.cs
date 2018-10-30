namespace Blaster.WebApi.Features.MyServices
{
    public static class UriStringUtil
    {
        public static string AddPath(this string uri, string path)
        {
            uri = uri.TrimEnd('/');
            path = path.TrimStart('/');
            return string.Format("{0}/{1}", uri, path);
        }
    }
}