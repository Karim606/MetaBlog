
using MetaBlog.Domain.Likes;

namespace MetaBlog.Api.Common.RouteConstraints
{
    public class LikeTargetTypeConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var routeValue)) { return false; }

            return Enum.TryParse(typeof(LikeTargetType),routeValue?.ToString(),true, out var _);
        }
    }
}
