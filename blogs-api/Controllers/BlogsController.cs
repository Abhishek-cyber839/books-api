using Microsoft.AspNetCore.Mvc;
using blogs_api.domain;
using blogs_api.Models;

[ApiController]
[Route("api/[controller]")]
public class BlogsController : Controller {
    private readonly ILogger _logger;
    private readonly BlogsContext _blogsContext;
    public BlogsController(ILogger<BlogsController> logger,BlogsContext blogsContext){
        _logger = logger;
        _blogsContext = blogsContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Blog>> getAllBlogs(int take = 10, int skip = 0){
        var result = _blogsContext.Blogs
                            .OrderBy(b => b.blogid)
                            .Skip(skip)
                            .Take(take);
        return Ok(result);
    }
     
    [HttpGet("blog/{id}")]
    public ActionResult<IEnumerable<Blog>> getBlogById(int id){
        var result = _blogsContext.Blogs
                            .OrderBy(b => b.blogid)
                            .Where(b => b.blogid == id)
                            .FirstOrDefault();
        if(result != null) return Ok(result);
        return NotFound("please try with a different id");
    }
}