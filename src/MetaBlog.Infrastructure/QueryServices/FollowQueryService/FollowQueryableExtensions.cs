using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Domain.Follows;
namespace MetaBlog.Infrastructure.QueryServices.FollowQueryService
{
    public enum FollowSearchOptions
    {
        FollowerName,
        FollowedName
    }
    public static class FollowQueryableExtensions
    {

        public static IQueryable<Follow> ApplySearch(this IQueryable<Follow> query,FollowSearchOptions option,string searchTerm)
        {
            searchTerm = searchTerm?.ToLower();
            if (!string.IsNullOrEmpty(searchTerm)&&option==FollowSearchOptions.FollowedName)
            {
                query = query.Where(f => f.Followed.firstName.Contains(searchTerm) || f.Followed.lastName.Contains(searchTerm));
            }
            
            else if(!string.IsNullOrEmpty(searchTerm)&&option==FollowSearchOptions.FollowerName)
            {
                query = query.Where(f => f.Follower.firstName.Contains(searchTerm) || f.Follower.lastName.Contains(searchTerm));
            }

            return query;
        }

        public static IQueryable<Follow> ApplySorting(this IQueryable<Follow> query, bool? sortDescending) {

            if (sortDescending.HasValue&&sortDescending==true)
            {
               query = query.OrderByDescending(f => f.Followed.firstName).ThenByDescending(f => f.Followed.lastName);
            }
            else
            {
                query = query.OrderBy(f => f.Followed.firstName).ThenBy(f => f.Followed.lastName);
            }

            return query;
        }

    }
}
