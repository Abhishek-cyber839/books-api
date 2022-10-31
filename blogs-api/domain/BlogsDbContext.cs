using Microsoft.EntityFrameworkCore;
using blogs_api.Models;
using blogs_api.Mapper;

namespace blogs_api.domain
{
   public class BlogsContext : DbContext
   {
       public BlogsContext(DbContextOptions<BlogsContext> options) : base(options){}

       public DbSet<Blog> Blogs { get; set; }

       protected override void OnModelCreating(ModelBuilder builder){
         base.OnModelCreating(builder);
         new BlogMap(builder.Entity<Blog>());
       }
   }
}