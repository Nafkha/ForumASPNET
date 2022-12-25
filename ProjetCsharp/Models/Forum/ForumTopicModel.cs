using ProjetCsharp.Models.Post;
using System.Collections.Generic;

namespace ProjetCsharp.Models.Forum
{
    public class ForumTopicModel
    {
        public ForumListingModel Forum { get; set; }

        public IEnumerable<PostListingModel> Posts { get; set; }
        public string SearchQuery { get; set; }
    }
}
