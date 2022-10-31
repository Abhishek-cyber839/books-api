using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using blogs_api.Models;

namespace blogs_api.Mapper
{
    public class BlogMap{
        // the names should match the column names in postgres table
        public BlogMap(EntityTypeBuilder<Blog> entityTypeBuilder){
            entityTypeBuilder.HasKey(x => x.blogid);
            entityTypeBuilder.ToTable("blogs");
            entityTypeBuilder.Property(x => x.authorname).HasColumnName("authorname");
            entityTypeBuilder.Property(x => x.description).HasColumnName("description");
            entityTypeBuilder.Property(x => x.blogcategory).HasColumnName("blogcategory");
            entityTypeBuilder.Property(x => x.title).HasColumnName("title");
            entityTypeBuilder.Property(x => x.numberofviewers).HasColumnName("numberofviewers");
        }
    }
}