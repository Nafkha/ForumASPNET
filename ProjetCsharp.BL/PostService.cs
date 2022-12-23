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
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Post pos)
        {
            _context.Add(pos);
            await _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditPostContent(int id, string newContent)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetAll()
        {
            throw new NotImplementedException();
        }

        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.Id == id).Include(post => post.User).Include(post => post.Replies)
                .ThenInclude(reply=>reply.User).Include(post => post.Forum).First();
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
           return _context.Forums.Where(forum => forum.Id == id).First().Posts;
        }
    }
}
