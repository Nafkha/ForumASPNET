using Microsoft.EntityFrameworkCore;
using ProjetCsharp.DAL;
using ProjetCsharp.DAL.Models;
using ProjetCsharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCsharp.BL
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _context;
        private readonly IPost _postService;


        public ForumService(ApplicationDbContext context, IPost postService)
        {
            _context = context;
            _postService = postService;
        }

        public async Task CreateAsync(Forum forum)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int forumId)
        {
            var forum = GetById(forumId);
            var posts = _postService.GetPostsByForum(forumId);
            foreach(var post in posts)
            {

                foreach(var reply in post.Replies)
                {
                    _context.Remove(reply);
                }

                _context.Remove(post);
                
            }
            
            _context.Remove(forum);
            await _context.SaveChangesAsync();    
                
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context.Forums.Include(Forum => Forum.Posts);
        }

        public IEnumerable<ApplicationUser> GetAllActiveUsers()
        {
            throw new NotImplementedException();
        }

        public Forum GetById(int id)
        {
            var forum = _context.Forums.Where(f => f.Id == id).Include(f=>f.Posts).ThenInclude(p=>p.User).Include(f=>f.Posts).ThenInclude(p=>p.Replies).ThenInclude(r=>r.User)
                .FirstOrDefault();
            return forum;
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }
    }
}
