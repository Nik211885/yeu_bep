using System.Text.Json;
using DeepSeek.ApiClient.Interfaces;
using DeepSeek.ApiClient.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using YeuBep.Const;
using YeuBep.Data;
using YeuBep.Entities;
using YeuBep.Queries;
using YeuBep.Services;
using YeuBep.ViewModels.Notification;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Controllers.Apis;

[ApiController]
[Route("api/recipe")]
public class RecipeApiController : ControllerBase
{
    private readonly ILogger<RecipeApiController> _logger;
    private readonly RecipeServices _recipeServices;
    private readonly RecipeQueries _recipeQueries;
    private readonly UserManager<User> _userManager;
    private readonly IDeepSeekClient _deepSeekClient;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly NotificationServices _notificationServices;

    public RecipeApiController(ILogger<RecipeApiController> logger, RecipeServices recipeServices,
        IHubContext<NotificationHub> hubContext, NotificationServices notificationServices,
        UserManager<User> userManager, RecipeQueries recipeQueries, IDeepSeekClient deepSeekClient)
    {
        _logger = logger;
        _recipeServices = recipeServices;
        _hubContext = hubContext;
        _notificationServices = notificationServices;
        _userManager = userManager;
        _recipeQueries = recipeQueries;
        _deepSeekClient = deepSeekClient;
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
    public async Task<IActionResult> Send(string? recipeId, CreateRecipeViewModel? model)
    {
        var result = await _recipeServices.SendApproveRecipeAsync(recipeId, model);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        var adminRole = await _userManager.GetUsersInRoleAsync(nameof(Role.Admin));
        var firstAdmin = adminRole.FirstOrDefault();
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        if (firstAdmin is not null && currentUser is not null)
        {
            var notificationModel = new CreateNotificationViewModel()
            {
                SendForUserId = firstAdmin.Id,
                Body = Template.SendApproveRecipeTemplate(result.Value.Title),
                Link = Template.SendApproveRecipeLinkOpenDetail(result.Value.Id),
                Title = "Phê duyệt công thức",
                CreatedDate = DateTimeOffset.UtcNow,
                NotificationSubject = NotificationSubject.Recipe
            };
            await _hubContext.Clients.User(firstAdmin.Id)
                .SendAsync("ReceiveMessage", JsonSerializer.Serialize(notificationModel));
            await _notificationServices.CreateNotificationAsync(notificationModel);
        }

        return Ok(result.Value);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] List<string> recipeId)
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

    [HttpPost("approve")]
    public async Task<IActionResult> Approve(string recipeId)
    {
        var result = await _recipeServices.ApproveRecipeAsync(recipeId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [HttpPost("reject")]
    public async Task<IActionResult> Reject(string recipeId)
    {
        var result = await _recipeServices.RejectRecipeAsync(recipeId);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [HttpGet("auto-complete-search")]
    public async Task<IActionResult> AutoCompleteSearch(string keyword)
    {
        var result = await _recipeQueries.SearchIntegrationAsync(keyword);
        return Ok(result);
    }

    [HttpPost("suggestion/recipe")]
    public async Task<IActionResult> Suggestion([FromBody]string request)
    {
        var getIngredientPrompt = AiPrompt.GetIngredient(request);
        string responseIngredient = await _deepSeekClient.SendMessageAsync(getIngredientPrompt, DeepSeekModel.V3);
        var ingredients = responseIngredient.Split(",")
            .Select(i => i.Trim())
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .ToArray();
        var recipesToPrompt = new Dictionary<string, RecipeViewModel>();
        foreach (var ingredient in ingredients)
        {
            var recipePagination = await _recipeQueries.GetRecipePaginationBySearchTermAsync(ingredient, 1, 5);
            foreach (var recipe in recipePagination.Items)
            {
                recipesToPrompt.TryAdd(recipe.Id, recipe);
            }
        }
        if (!recipesToPrompt.Any())
        {
            return Ok(new
            {
                analysis = "Rất tiếc, hệ thống chưa có công thức nào phù hợp với nguyên liệu bạn đề cập. Vui lòng thử lại với nguyên liệu khác!",
                suggestedRecipes = new List<RecipeViewModel>()
            });
        }
        var analyzeAndSuggestPrompt = AiPrompt.AnalyzeAndSuggestRecipe(request, responseIngredient, recipesToPrompt);
        string aiResponse = await _deepSeekClient.SendMessageAsync(analyzeAndSuggestPrompt, DeepSeekModel.V3);
        var (analysisText, suggestedIds) = ParseAiResponse(aiResponse);
        var suggestedRecipes = suggestedIds
            .Where(id => recipesToPrompt.ContainsKey(id))
            .Select(id => recipesToPrompt[id])
            .ToList();
        return Ok(new
        {
            userAnalysis = analysisText,
            suggestedRecipes = suggestedRecipes
        });
        (string analysisText, List<string> recipeIds) ParseAiResponse(string response)
        {
            try
            {
                var analysisMatch = System.Text.RegularExpressions.Regex.Match(
                    response, 
                    @"\[ANALYSIS\](.*?)\[/ANALYSIS\]", 
                    System.Text.RegularExpressions.RegexOptions.Singleline
                );
        
                var idsMatch = System.Text.RegularExpressions.Regex.Match(
                    response, 
                    @"\[IDS\](.*?)\[/IDS\]"
                );
        
                string analysisTextX = analysisMatch.Success 
                    ? analysisMatch.Groups[1].Value.Trim() 
                    : "Dựa trên yêu cầu của bạn, hệ thống đã tìm được một số món ăn phù hợp.";
        
                List<string> recipeIds = idsMatch.Success 
                    ? idsMatch.Groups[1].Value.Split(",")
                        .Select(id => id.Trim())
                        .Where(id => !string.IsNullOrWhiteSpace(id))
                        .ToList()
                    : new List<string>();
        
                return (analysisTextX, recipeIds);
            }
            catch
            {
                return ("Không thể phân tích phản hồi từ AI.", new List<string>());
            }
        }
    }
}