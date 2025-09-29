using MetaBlog.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.RepositoriesInterfaces
{
    public interface IDomainUserRepository
    {
        public Task AddUserAsync(User user);
        public Task<User>GetByIdAsync(Guid id);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(User user);
        public Task SaveChangesAsync();

    }
}
