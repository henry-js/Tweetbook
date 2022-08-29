using Tweetbook.Domain;

namespace Tweetbook.Services;

public interface IPostService
{
    public List<Post> GetPosts();
    public Post GetPostById(Guid postId);
}
