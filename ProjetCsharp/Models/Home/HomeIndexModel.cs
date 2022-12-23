using ProjetCsharp.Models.Post;
using System.Collections.Generic;

namespace ProjetCsharp.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
    }
}
