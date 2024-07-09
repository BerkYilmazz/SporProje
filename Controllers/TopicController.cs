using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporProje.Models;

namespace SporProje.Controllers
{
    [Authorize]
    public class TopicController : Controller
    {
        private readonly SporDbContext context;
        public TopicController()
        {
            context = new SporDbContext();
        }

        [HttpGet]
        public async Task<IActionResult> MyTopics()
        {
            var user = Convert.ToInt32(User.FindFirst(x => x.Type == "UserId").Value);

            var topic = await context.Topics.Where(t=>t.UserId==user).Include(u=>u.User).ToListAsync();

            return View(topic);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTopic()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult CreateTopic(Topic topic)
        {
            if (topic==null)
            {
                return RedirectToAction("MyTopics");
            }
            else
            {
                var t = new Topic
                {
                    Title = topic.Title,
                    ContentText = topic.ContentText,
                    UserId = Convert.ToInt32(User.FindFirst(x => x.Type == "UserId").Value),
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now)
                };

                context.Topics.Add(t);
                context.SaveChanges();
                return RedirectToAction("MyTopics");
            }
        
        }

        [HttpGet]
        public async Task<IActionResult> EditTopic(int id) 
        {
            var topic = await context.Topics.FirstOrDefaultAsync(x => x.TopicId == id);

            return View(topic);
        }

        [HttpPost]
        public async Task<IActionResult> EditTopic(Topic topic)
        {
            
            if (ModelState.IsValid)
            {
                context.Topics.Update(topic);
                context.SaveChanges();
                return RedirectToAction("MyTopics");
            }
            else
            {
                return View(topic);
            }
        
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await context.Topics.FirstOrDefaultAsync(x => x.TopicId == id);
            if (topic == null)
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                return View(topic);
            }

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDeleteTopic(int id)
        {
            var topic = await context.Topics.FirstOrDefaultAsync(x => x.TopicId == id);
            if (topic == null)
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                var comment = await context.Comments.Where(t => t.TopicId == id).ToListAsync();
                context.Comments.RemoveRange(comment);
                context.Topics.Remove(topic);
                context.SaveChanges();
                return RedirectToAction("MyTopics", "Topic");
            }
        
        }

    }

}
