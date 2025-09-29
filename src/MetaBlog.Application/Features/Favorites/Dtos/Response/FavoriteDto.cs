using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Favorites.Dtos.Response
{
    public class FavoriteDto
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Title { get; set; }

    }
}
