using MetaBlog.Domain.Comments;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Favorites;
using MetaBlog.Domain.Likes;
using MetaBlog.Domain.Posts;
using MetaBlog.Domain.Users;
using MetaBlog.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Data
{
    public static class AppDbContextIntialiser
    {
        public static async Task Init(this WebApplication app) {

            using var scope = app.Services.CreateScope();
            var intialiser = scope.ServiceProvider.GetRequiredService<DbIntialiser>();
            await  intialiser.initAsync();
            await intialiser.SeedAsync();   
        }
    }


    public class DbIntialiser(AppDbContext appDbContext,ILogger<DbIntialiser>Logger,UserManager<IdentityAppUser>userManager,RoleManager<IdentityRole<Guid>>roleManager)
    {
        public async Task initAsync()
        {
            try {
                appDbContext.Database.EnsureCreated();
            
            }
            catch (Exception ex) { Logger.LogError(ex, "An error occurred while initialising database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            using var transaction = await appDbContext.Database.BeginTransactionAsync();
            try
            {
                
                await TrySeeding();
                await transaction.CommitAsync();
            }
            catch (Exception ex) { 
                await transaction.RollbackAsync();
                Logger.LogError(ex, "An error occurred while seeding database.");
                throw;
            }
        }

        private async Task TrySeeding()
        {
            await SeedRolesAndUsers();
            await SeedPostsAndInteractions();
        }

        private async Task AddRole(string roleName)
        {
            if(roleManager.Roles.All(r => r.Name != roleName)&&roleName!=null)
            {
                var role = new IdentityRole<Guid>() { Id=new Guid(),
                Name = roleName
                };
               var result = await roleManager.CreateAsync(role);
                if(result.Succeeded)
                {
                    Logger.LogInformation("Role {RoleName} created successfully", roleName);
                }
                else
                {
                    Logger.LogWarning("Failed to create role {RoleName}. Errors: {Errors}", roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            
        }
        private async Task SeedRolesAndUsers()
        {
            // 1. Add roles
            await AddRole("Admin");
            await AddRole("User");

            // 2. Add users (IdentityAppUser + domain User)
            await AddAppUser(
                Email: "admin@example.com",
                Password: "Admin@123456",
                phoneNumber: "1234567890",
                role: "Admin"
            );

            await AddAppUser(
                Email: "user@example.com",
                Password:"User@123456",
                phoneNumber: "0987654321",
                role: "User"
            );

            // 3. Add domain Users linked to IdentityAppUser
            await AddDomainUser("Admin","User","admin@example.com", DateOnly.FromDateTime(new DateTime(1980, 1, 1)));
            await AddDomainUser("Normal","User","user@example.com", DateOnly.FromDateTime(new DateTime(1995, 6, 15)));
        }


        private async Task AddAppUser(string Email,string Password,string phoneNumber,string role)
        {
            bool NotExist = userManager.Users.All(u => u.Email != Email)&&Email!=null;
            if (NotExist) {
                var appUser = new IdentityAppUser()
                {
                    Id = new Guid(),
                    Email=Email,
                    UserName=Email,
                    NormalizedEmail=Email,
                    PhoneNumber=phoneNumber,
                    
                };
                var result  = await userManager.CreateAsync(appUser,Password);
                if (result.Succeeded) {
                    Logger.LogInformation("User with Email {Email} created successfully", Email);
                   var added = await userManager.AddToRoleAsync(appUser, role);
                    if(added.Succeeded)
                    {
                        Logger.LogInformation("User with Email {Email} added to role {Role} successfully", Email,role);
                    }
                    else
                    {
                        Logger.LogWarning("Failed to add user with Email {Email} to role {Role}. Errors: {Errors}", Email, role, string.Join(", ", added.Errors.Select(e => e.Description)));
                    }
                }
            }

        }

        private async Task AddDomainUser(string firstName,string lastName,string email, DateOnly dob)
        {
            var appUser = await userManager.FindByEmailAsync(email);
            if (appUser == null)
            {
                Logger.LogWarning("Cannot create domain User for non-existing IdentityAppUser {Email}", email);
                return;
            }

            var exists = await appDbContext.Users.AnyAsync(u => u.Id == appUser.Id);
            if (!exists)
            {
                var user = User.Create(appUser.Id, dob,firstName,lastName);
                appDbContext.Users.Add(user);
                await appDbContext.SaveChangesAsync();
                Logger.LogInformation("Domain User created for {Email}", email);
            }
        }

        private async Task SeedPostsAndInteractions()
        {
            // Check if any posts exist
            if (appDbContext.Posts.Any()) return;

            var users = await appDbContext.Users.ToListAsync();

            // 1. Create posts
            var post1 = Post.Create(Guid.NewGuid(), "First Post", "Content for first post", users[0].Id);
            var post2 = Post.Create(Guid.NewGuid(), "Second Post", "Content for second post", users[1].Id);
            appDbContext.Posts.AddRange(post1, post2);

            // 2. Likes
            var like1 = Like.Create(Guid.NewGuid(), post1.Id,LikeTargetType.Post, users[1].Id);
            var like2 = Like.Create(Guid.NewGuid(), post2.Id,LikeTargetType.Post, users[0].Id);
            appDbContext.Likes.AddRange(like1, like2);

            // 3. Favorites
            var fav1 =  Favorite.Create(Guid.NewGuid(), post1.Id, users[1].Id);
            var fav2 =  Favorite.Create(Guid.NewGuid(), post2.Id, users[0].Id);
            appDbContext.Favorites.AddRange(fav1, fav2);

            // 4. Comments
            var comment1 = Comment.Create(Guid.NewGuid(), post1.Id, users[1].Id, "Great post!", null);
            var comment2 = Comment.Create(Guid.NewGuid(), post2.Id, users[0].Id, "Nice article", null);
            appDbContext.Comments.AddRange(comment1, comment2);

            await appDbContext.SaveChangesAsync();
            Logger.LogInformation("Posts, Likes, Favorites, and Comments seeded successfully.");
        }

    }
}
