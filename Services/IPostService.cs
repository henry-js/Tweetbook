using Tweetbook.Domain;

namespace Tweetbook.Services;

public interface IPostService
{
    public Task<bool> CreatePostAsync(Post post);
    public Task<List<Post>> GetPostsAsync();
    public Task<Post> GetPostByIdAsync(Guid postId);

    Task<bool> UpdatePostAsync(Post postToUpdate);
    Task<bool> DeletePostAsync(Guid postId);
}
