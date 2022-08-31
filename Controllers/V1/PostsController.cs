using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PostsController : ControllerBase
{
    IPostService _postService;
    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet(ApiRoutes.Posts.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _postService.GetPostsAsync());
    }

    [HttpGet(ApiRoutes.Posts.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid postId)
    {
        var post = await _postService.GetPostByIdAsync(postId);

        if (post == null)
            return NotFound();

        return Ok(post);
    }

    [HttpPut(ApiRoutes.Posts.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
    {
        var post = new Post
        {
            Id = postId,
            Name = request.Name
        };

        var updated = await _postService.UpdatePostAsync(post);

        if (updated)
            return Ok(post);

        return NotFound();

    }

    [HttpPost(ApiRoutes.Posts.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
    {
        var post = new Post { Name = postRequest.Name };

        await _postService.CreatePostAsync(post);

        var baseUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        var locationUri = baseUri + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

        var response = new PostResponse { Id = post.Id, Name = post.Name };

        return Created(locationUri, response);
    }

    [HttpDelete(ApiRoutes.Posts.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid postId)
    {
        var deleted = await _postService.DeletePostAsync(postId);

        if (deleted)
            return NoContent();

        return NotFound();
    }
}
