using MetaBlog.Domain.RefreshTokens;
using MetaBlog.Domain.RepositoriesInterfaces;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        public async Task AddTokenAsync(RefreshToken refreshToken)
        {
           context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetTokenByIdAsync(Guid Id)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == Id);
        }
        public async Task<RefreshToken> GetByHashTokenAsync(string hashToken)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.token==hashToken);
        }
        public async Task RemoveTokenAsync(RefreshToken refreshToken)
        {
            context.RefreshTokens.Remove(refreshToken);
            await context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
