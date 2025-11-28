using YeuBep.Entities;
using YeuBep.ViewModels.Account;
using YeuBep.ViewModels.Comment;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.Const;

public static class FakeData
{
    public static readonly List<RecipeViewModel> Recipe =
    [
        new RecipeViewModel
    {
        Id = Guid.NewGuid().ToString(),
        CreatedDate = DateTimeOffset.Now.AddDays(-1),
        CreatedBy = new AccountInfo
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "user1",
            Bio = "Bio of user1",
            Avatar = "https://i.pravatar.cc/150?img=1"
        },
        Avatar = "https://th.bing.com/th/id/R.6ce900059cd52be65130d44b9039c828?rik=A0yjFrs8DvnmKw&pid=ImgRaw&r=0",
        Title = "Recipe Title 1",
        Description = "This is the description for recipe 1.",
        PortionCount = "2 servings",
        Slug = "recipe-title-1",
        TimeToCook = "20 mins",
        CountFavorite = 10,
        CountRatingPoint = 2,
        TotalRatingPoint = 7,
        IngredientPart = new List<IngredientPart>
        {
            new IngredientPart
            {
                Title = "Main Ingredients",
                Ingredients = new List<string> { "Ingredient A", "Ingredient B" }
            },
            new IngredientPart
            {
                Title = "Spices",
                Ingredients = new List<string> { "Salt", "Pepper" }
            }
        },
        DetailInstructionSteps = new List<DetailInstructionStep>
        {
            new DetailInstructionStep
            {
                Instructions = "Step 1 for recipe 1",
                ImageDescription = "https://picsum.photos/seed/step1-1/300/200"
            },
            new DetailInstructionStep
            {
                Instructions = "Step 2 for recipe 1",
                ImageDescription = "https://picsum.photos/seed/step1-2/300/200"
            }
        },
        Comments = new List<CommentViewModel>
        {
            new CommentViewModel
            {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTimeOffset.UtcNow,
                RecipeId = Guid.NewGuid().ToString(),
                CommentText = "This is a comment on recipe 1",
                CreatedBy = new AccountInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "commenter1",
                    Bio = "Bio of commenter1",
                    Avatar = "https://i.pravatar.cc/150?img=11"
                }
            }
        },
        RecipeStatus = RecipeStatus.Accept
    },

    new RecipeViewModel
    {
        Id = Guid.NewGuid().ToString(),
        CreatedDate = DateTimeOffset.Now.AddDays(-2),
        CreatedBy = new AccountInfo
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "user2",
            Bio = "Bio of user2",
            Avatar = "https://i.pravatar.cc/150?img=2"
        },
        Avatar = "https://img1.wsimg.com/isteam/stock/2982",
        Title = "Recipe Title 2",
        Description = "This is the description for recipe 2.",
        PortionCount = "3 servings",
        Slug = "recipe-title-2",
        TimeToCook = "25 mins",
        CountFavorite = 15,
        CountRatingPoint = 3,
        TotalRatingPoint = 10,
        IngredientPart = new List<IngredientPart>
        {
            new IngredientPart
            {
                Title = "Main Ingredients",
                Ingredients = new List<string> { "Ingredient C", "Ingredient D" }
            },
            new IngredientPart
            {
                Title = "Spices",
                Ingredients = new List<string> { "Salt", "Chili" }
            }
        },
        DetailInstructionSteps = new List<DetailInstructionStep>
        {
            new DetailInstructionStep
            {
                Instructions = "Step 1 for recipe 2",
                ImageDescription = "https://picsum.photos/seed/step2-1/300/200"
            },
            new DetailInstructionStep
            {
                Instructions = "Step 2 for recipe 2",
                ImageDescription = "https://picsum.photos/seed/step2-2/300/200"
            }
        },
        Comments = new List<CommentViewModel>
        {
            new CommentViewModel
            {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTimeOffset.UtcNow,
                RecipeId = Guid.NewGuid().ToString(),
                CommentText = "This is a comment on recipe 2",
                CreatedBy = new AccountInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "commenter2",
                    Bio = "Bio of commenter2",
                    Avatar = "https://i.pravatar.cc/150?img=12"
                }
            }
        },
        RecipeStatus = RecipeStatus.Accept
    }
    ];

    public static readonly List<CommentViewModel> Comment =
    [
    ];
}