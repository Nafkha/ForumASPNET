using ProjetCsharp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCsharp.DAL
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();

        Task SetProfileImage(string id, Uri uri);
        Task IncrementRating(string userId, Type type);
    }
}
