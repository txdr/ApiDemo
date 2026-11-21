using ApiDemo.Models;
using ApiDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDemo.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {

        // Class Variables
        private readonly BlogDbContext _db;

        // Constructor
        public PostsController(BlogDbContext db) => _db = db;

        //APIs
        // GET: /api/posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAll()
        {
            var posts = await _db.Posts
                .OrderByDescending(p => p.CreatedUTC)
                .ToListAsync();
            return Ok(posts);
        }

        // GET: /api/posts/{id}
        public async Task<ActionResult<Post>> GetById(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        // POST: /api/posts
        public async Task<ActionResult<Post>> Create(Post post)
        {
            // Validate the model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create and save the post
            post.CreatedUTC = DateTime.UtcNow;
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            // Return 201 created
            return CreatedAtAction(nameof(GetById), new {id = post.Id}, post);
        }

    }
}
