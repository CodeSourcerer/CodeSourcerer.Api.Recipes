using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeSourcerer.Api.Recipes.Models;
using CodeSourcerer.RecipeContext.RecipeContext;

namespace CodeSourcerer.Api.Recipes.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly recipesContext _dbContext;

        public IngredientService(recipesContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.Ingredient> AddAsync(Models.Ingredient ingredient, CancellationToken token = default)
        {
            var ingredientEntity = Models.Ingredient.ToEntity(ingredient);

            await _dbContext.AddAsync(ingredientEntity, token).ConfigureAwait(false);

            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            return Models.Ingredient.FromEntity(ingredientEntity);
        }
    }
}
