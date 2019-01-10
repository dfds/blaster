using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Blaster.WebApi
{
    public class CreatedAtRouteResult<T> : IConvertToActionResult
    {
        private readonly string _routeName;
        private readonly object _routeValues;

        public CreatedAtRouteResult(string routeName, object routeValues, T value)
        {
            _routeName = routeName;
            _routeValues = routeValues;
            Value = value;
        }

        public T Value { get; }

        public IActionResult Convert()
        {
            return new CreatedAtRouteResult(_routeName, _routeValues, Value);
        }
    }
}