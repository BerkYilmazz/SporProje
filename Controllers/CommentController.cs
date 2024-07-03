using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporProje.Models;

namespace SporProje.Controllers
{

    [Authorize]
    public class CommentController : Controller
    {
        private readonly SporDbContext context;

        public CommentController()
        {
            context = new SporDbContext();
        }


        [HttpGet]
        public IActionResult ListComments(int id)
        {
            // var c = context.Likes.FirstOrDefault(x=>x.PhotoId==photoId);
            var comments = context.Comments.Where(x => x.TopicId == id).Include(u => u.User).ToList();
            return View(comments);
        }


    }
}
