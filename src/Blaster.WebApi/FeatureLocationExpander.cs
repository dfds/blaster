using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Blaster.WebApi
{
    public class FeatureLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var yPlural = Regex.Replace(context.ControllerName, @"^(.+)y$", "$1");
            var vPlural = Regex.Replace(context.ControllerName, @"^(.+)y(v[0-9]*)$", "$1ies$2");
            
            var temp = new[]
            {
                "/Features/{1}/{0}.cshtml",
                "/Features/{1}s/{0}.cshtml",
                $"/Features/{yPlural}ies/{{0}}.cshtml",
                $"/Features/{vPlural}/{{0}}.cshtml",
                "/Features/Shared/{0}.cshtml",
            };

            return temp;
        }
    }
}