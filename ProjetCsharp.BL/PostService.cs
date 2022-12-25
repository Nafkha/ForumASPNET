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

        public async Task AddReply(PostReply reply)
        {
            _context.PostReplies.Add(reply);
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
            return _context.Posts.Include(post => post.User).Include(post => post.Replies).ThenInclude(reply => reply.User).Include(post => post.Forum);
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

        public IEnumerable<Post> GetFilteredPosts(int id, string searchQuery)
        {
            

            var forum = _context.Forums.Find(id);
            return string.IsNullOrEmpty(searchQuery)? forum.Posts : forum.Posts.Where(post => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
        }

        public IEnumerable<Post> GetLatestPosts(int nPosts)
        {
            return GetAll().OrderByDescending(post => post.Created).Take(nPosts);
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
           return _context.Forums.Where(forum => forum.Id == id).First().Posts;
        }
    }
}
