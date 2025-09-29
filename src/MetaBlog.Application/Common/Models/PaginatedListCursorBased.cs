using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Models
{
    public class PaginatedListCursorBased<T1,T2>
    {
        public int PageSize { get; init; }
        public T2 NextCursor{ get; init; }

        public IReadOnlyCollection<T1>? Items { get; init; }
    }
}
