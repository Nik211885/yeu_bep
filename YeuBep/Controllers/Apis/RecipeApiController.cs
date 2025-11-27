using FluentResults;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Services;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Controllers.Apis;

[ApiController]
[Route("api/recipe")]
public class RecipeApiController : ControllerBase
{
    private readonly ILogger<RecipeApiController> _logger;
    private readonly RecipeServices _recipeServices;

    public RecipeApiController(ILogger<RecipeApiController> logger, RecipeServices recipeServices)
    {
        _logger = logger;
        _recipeServices = recipeServices;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateRecipeViewModel recipe)
    {
        var result = await _recipeServices.CreateRecipeAsync(recipe);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(string recipeId, CreateRecipeViewModel recipe)
    {
        var result = await _recipeServices.UpdateRecipeAsync(recipeId, recipe);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
    [HttpPost("send")]
    public async Task<IActionResult> Send(string? recipeId, CreateRecipeViewModel model)
    {
        var result = await _recipeServices.SendApproveRecipeAsync(recipeId, model);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string recipeId)
    {
        var result = await _recipeServices.DeleteRecipeAsync(recipeId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return NoContent();
    }

    [HttpPost("unpublish")]
    public async Task<IActionResult> UnpublishAsync(string recipeId)
    {
        var result = await _recipeServices.UnpublishRecipeAsync(recipeId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }
}