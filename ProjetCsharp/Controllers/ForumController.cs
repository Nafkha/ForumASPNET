using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using ProjetCsharp.DAL;
using ProjetCsharp.DAL.Models;
using ProjetCsharp.Models.Forum;
using ProjetCsharp.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjetCsharp.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ForumController(IForum forumService, IPost postService,IUpload uploadService,IConfiguration configuration)
        {
            _forumService = forumService;
            _postService = postService;
            _uploadService = uploadService;
            _configuration = configuration;
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

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);
            var posts = new List<Post>();

            if (!String.IsNullOrEmpty(searchQuery))
            {
                 posts = _postService.GetFilteredPosts(id, searchQuery).ToList();
            }
            else {
                posts = forum.Posts.ToList();
            }
    
            

           

            var postingListing = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                AuthorName = post.User.UserName,
                Title = post.Title,
                DatePosted = post.Created,
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)



            });
            var model = new ForumTopicModel
            {
                Posts = postingListing,
                Forum = BuildForumListing(forum)

            };
            return View(model);
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }
        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }
        public IActionResult Create()
        {
            var model = new AddForumModel();
            return View(model);
        }
        [HttpPost]
        /*public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "/images/users/default.png";
            if (model.ImageUploade != null)
            {
                var blockBlob = UploadForumImage(model.ImageUploade);
                imageUri = blockBlob.Uri.AbsoluteUri;
            }
            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };
            await _forumService.CreateAsync(forum);
            return RedirectToAction("Index", "Forum");
   
        }*/
        [HttpPost]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            var container = _uploadService.GetBlobContainer(connectionString);
            var contentDisposition = ContentDispositionHeaderValue.Parse(model.ImageUploade.ContentDisposition);
            var filename = contentDisposition.FileName.ToString().Trim('"');
            var blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.UploadFromStreamAsync(model.ImageUploade.OpenReadStream());
            var forum = BuildForum(model, blockBlob.Uri.ToString());
            await _forumService.CreateAsync(forum);
            return RedirectToAction("Index", "Forum");


        }

        private Forum BuildForum(AddForumModel model, string v)
        {
            return new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = v
            };
        }

        private CloudBlockBlob UploadForumImage(IFormFile imageUpload)
        {
            // Connect to an Azure Storage Container

            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");

            // Get Blob Container
            var container = _uploadService.GetBlobContainer(connectionString);

            // Parse the content diposition response header

            var contentDisposition = ContentDispositionHeaderValue.Parse(imageUpload.ContentDisposition);

            // Grab the filename

            var filename = contentDisposition.FileName.Trim('"');


            //Get a reference to a block blob

            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadFromStreamAsync(imageUpload.OpenReadStream());


            return blockBlob;
        }
        [HttpPost]
        public async Task<IActionResult> DeleteForum(int id)
        {
            _forumService.DeleteAsync(id).Wait();
            return RedirectToAction("Index", "Forum");
        }
        
    }
}