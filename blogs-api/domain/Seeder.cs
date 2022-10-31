using AutoFixture;
using blogs_api.Models;

namespace blogs_api.domain
{
   public static class Seeder
   {
       public static void Seed(this BlogsContext blogsContext)
       {
           if (!blogsContext.Blogs.Any())
           {
                Fixture fixture = new Fixture();
                fixture.Customize<Blog>(blog => blog.Without(b => b.blogid));
                    //--- The next two lines add 100 rows to your database
                List<Blog> blogs = fixture.CreateMany<Blog>(100).ToList();
                foreach(var blog in blogs){
                    blog.authorname = blog.authorname.Substring(0,40); // as we have set max-length of author column to 40
                }
                blogsContext.AddRange(blogs);
                blogsContext.SaveChanges();
          }
       }
   }
}