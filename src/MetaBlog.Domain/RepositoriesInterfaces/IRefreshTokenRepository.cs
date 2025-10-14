using MetaBlog.Domain.RefreshTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.RepositoriesInterfaces
{
    public interface IRefreshTokenRepository
    {
        public Task AddTokenAsync(RefreshToken refreshToken);
        public Task<RefreshToken> GetByHashTokenAsync(string hashToken);
        public Task<RefreshToken> GetTokenByIdAsync(Guid Id);
        public Task RemoveTokenAsync(RefreshToken refreshToken);

        public Task SaveChangesAsync( );
    }
}
