using MetaBlog.Domain.RepositoriesInterfaces;
using MetaBlog.Domain.Users;
using MetaBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Repositories
{
    public class DomainUserRepository(AppDbContext context) : IDomainUserRepository
    {

        public async Task AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);
            await SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            context.Users.Remove(user);
            await SaveChangesAsync();

        }

       

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(u=>u.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            context.Update(user);
            await SaveChangesAsync();
        }
    }
}
