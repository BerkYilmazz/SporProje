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
        public IActionResult AddComment(int id)
        {

            return View(id);
        }

        [HttpPost]
        public IActionResult AddComment([FromForm] Comment comment)
        {
            if (comment == null)
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                return RedirectToAction("MyTopics", "Topic");
            }

        }

        [HttpGet]
        public IActionResult DeleteComment(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                var c = context.Comments.FirstOrDefault(x => x.CommentId == id);
                context.Comments.Remove(c);
                context.SaveChanges();
                return RedirectToAction("MyTopics", "Topic");
            }
        }

        [HttpPost]
        public IActionResult EditComment( Comment comment)
        {
            if (comment != null)
            {
                //I left that on purpose on here

                //context.Comments.Update(comment);
                //context.SaveChanges();
                //return RedirectToAction("GetImage", "Post");

                Comment c = context.Comments.FirstOrDefault(x => x.CommentId == comment.CommentId && x.UserId == comment.UserId);
                c.CommentText = comment.CommentText;
                context.Comments.Update(c);
                context.SaveChanges();
                return RedirectToAction("MyTopics", "Topic");
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            var c = await context.Comments.FirstOrDefaultAsync(x => x.CommentId == id);

            if (c == null)
            {
                return RedirectToAction("MyTopics","Topic");
            }
            else
            {
                return View("EditComment", c);
            }
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
