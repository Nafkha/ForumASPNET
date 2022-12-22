using Microsoft.AspNetCore.Mvc;
using ProjetCsharp.DAL;
using ProjetCsharp.DAL.Models;
using ProjetCsharp.Models.Forum;
using System.Collections.Generic;
using System.Linq;

namespace ProjetCsharp.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        public ForumController(IForum forumService)
        {
            _forumService = forumService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll().Select(forum => new ForumListingModel {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description

            });
            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }
    }
}