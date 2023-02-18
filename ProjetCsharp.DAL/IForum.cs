using ProjetCsharp.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCsharp.DAL
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();
        IEnumerable<ApplicationUser> GetAllActiveUsers();

        Task CreateAsync(Forum forum);
        Task DeleteAsync(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);

    }
}
