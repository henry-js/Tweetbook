using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1;

public class PostsController : ControllerBase
{
    IPostService postService;
    public PostsController(IPostService postService)
    {
        this.postService = postService;
    }

    [HttpGet(ApiRoutes.Posts.GetAll)]
    public IActionResult GetAll()
    {
        return Ok(postService.GetPosts());
    }

    [HttpGet(ApiRoutes.Posts.Get)]
    public IActionResult Get([FromRoute] Guid postId)
    {
        var post = postService.GetPostById(postId);

        if (post == null)
            return NotFound();

        return Ok(post);
    }

    [HttpPost(ApiRoutes.Posts.Create)]
    public IActionResult Create([FromBody] CreatePostRequest postRequest)
    {
        var post = new Post { Id = postRequest.Id };

        if (post.Id != Guid.Empty)
            post.Id = Guid.NewGuid();

        postService.GetPosts().Add(post);

        var baseUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        var locationUri = baseUri + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

        var response = new PostResponse { Id = post.Id };

        return Created(locationUri, response);
    }
}