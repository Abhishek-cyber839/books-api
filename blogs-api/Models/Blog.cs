namespace blogs_api.Models
{
  // the names should match the column names in postgres table
    public class Blog {
        public int blogid { get; set; }
        public string authorname { get; set; } = "";
        public BlogCategory blogcategory { get; set; }
        public string? description { get; set; }
        public string title { get; set; } =  "";
        public long numberofviewers { get; set; }
  }    
}
