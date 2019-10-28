using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lakshmi.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public ICollection<Photo> Photos { get; set; } //Коллекция фотографий юзера
        public ICollection<Comment> Comments { get; set; }//Коллекция коментариев юзера
        public ApplicationUser()
        {
            Photos = new List<Photo>();
            Comments = new List<Comment>();
        }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public Photo()
        {
            Comments = new List<Comment>();
        }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
    public class Comment
    {
        public int Id { get; set; }
        public string Record { get; set; }//Cам коментарий

        public int PhotoId { get; set; } //Вторичный ключ для айди фотки
        public Photo Photo { get; set; }

        public string ApplicationUserId { get; set; } //Вторичный ключ для айди пользователя
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Photo> Photos { get; set; }
        
        public DbSet<Comment> Comments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}