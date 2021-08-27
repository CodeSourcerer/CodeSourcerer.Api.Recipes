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

            var recipeEntity = Models.Recipe.ToEntity(recipe);

            await _dbContext.AddAsync(recipeEntity, token).ConfigureAwait(false);

            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            return Models.Recipe.FromEntity(recipeEntity);
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

        public async Task<Models.Recipe> UpdateAsync(Models.Recipe recipe, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var r = await _dbContext.FindAsync<Recipe>(new object[] { recipe.Id }, cancellationToken: token);

            if (r == null)
            {
                return null;
            }

            r = recipe.ToEntity(r);
            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            return Models.Recipe.FromEntity(r);
        }

        public async Task<IEnumerable<Models.Recipe>> GetAllAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var recipes = from r in _dbContext.Recipes
                          select Models.Recipe.FromEntity(r);

            return recipes;
        }
    }
}
