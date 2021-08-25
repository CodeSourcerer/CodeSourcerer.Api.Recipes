using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeSourcerer.RecipeContext.RecipeContext;

namespace CodeSourcerer.Api.Recipes.Services
{
    public class RecipesService : IRecipeService
    {
        private readonly recipesContext _dbContext;

        public RecipesService(recipesContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.Recipe> AddAsync(Models.Recipe recipe, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var newRecipe = new Recipe
            {
                Name     = recipe.Name,
                Notes    = recipe.Notes,
                Servings = recipe.Servings,
                Tags     = recipe.Tags
            };

            var addedRecipe = await _dbContext.AddAsync(newRecipe, token).ConfigureAwait(false);

            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            return Models.Recipe.FromEntity(addedRecipe.Entity);
        }

        public async Task<Models.Recipe> GetAsync(int id, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var r = (from recipe in _dbContext.Recipes
                     where recipe.Id == id
                     select recipe).SingleOrDefault();

            if (r == null)
            {
                return null;
            }

            return Models.Recipe.FromEntity(r);
        }
    }
}
