using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeSourcerer.RecipeContext.RecipeContext;
using Microsoft.EntityFrameworkCore;

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

            foreach (var ingredient in recipe.Ingredients)
            {
                var recipeIngredient = new RecipeIngredient
                {
                    IngredientId = ingredient.Ingredient.Id,
                    Recipe = recipeEntity,
                    Amount = ingredient.Amount,
                    Unit = ingredient.Unit
                };
                await _dbContext.AddAsync(recipeIngredient, token).ConfigureAwait(false);
            }
            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            return Models.Recipe.FromEntity(recipeEntity, true);
        }

        public async Task<Models.Recipe> GetAsync(int id, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var r = (from recipe in _dbContext.Recipes.Include(r => r.RecipeIngredients)
                                                      .ThenInclude(ri => ri.Ingredient)
                     where recipe.Id == id
                     select recipe).SingleOrDefault();

            if (r == null)
            {
                return null;
            }

            return Models.Recipe.FromEntity(r, true);
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

            return Models.Recipe.FromEntity(r, true);
        }

        public async Task<IEnumerable<Models.Recipe>> GetAllAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var recipes = from r in _dbContext.Recipes.Include(r => r.RecipeIngredients)
                                                      .ThenInclude(ri => ri.Ingredient)
                          select Models.Recipe.FromEntity(r, true);

            return recipes;
        }

        public async Task<Models.Recipe> AddIngredientAsync(int recipeId, int ingredientId, double amount, string unit, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var recipe = await _dbContext.FindAsync<Recipe>(new object[] { recipeId }, cancellationToken: token).ConfigureAwait(false);
            if (recipe == null)
                return null;

            var ingredient = await _dbContext.FindAsync<Ingredient>(new object[] { ingredientId }, cancellationToken: token).ConfigureAwait(false);
            if (ingredient == null)
                return null;

            var recipeIngredient = new Models.RecipeIngredient
            {
                RecipeId = recipeId,
                Ingredient = Models.Ingredient.FromEntity(ingredient),
                Amount = amount,
                Unit = unit
            };

            return await AddIngredientAsync(recipeIngredient, token).ConfigureAwait(false);
        }

        public async Task<Models.Recipe> AddIngredientAsync(Models.RecipeIngredient recipeIngredient, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var ri = recipeIngredient.ToEntity();

            await _dbContext.AddAsync(ri, token).ConfigureAwait(false);

            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            var recipe = await (from r in _dbContext.Recipes.Include(r2 => r2.RecipeIngredients)
                                                            .ThenInclude(r2 => r2.Ingredient)
                                where r.Id == ri.RecipeId
                                select r).SingleAsync(token).ConfigureAwait(false);

            return Models.Recipe.FromEntity(recipe, true);
        }

        public async Task<Models.Recipe> DeleteIngredientAsync(int recipeId, int recipeIngredientId, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var ri = await _dbContext.FindAsync<RecipeIngredient>(new object[] { recipeIngredientId }, cancellationToken: token).ConfigureAwait(false);

            if (ri == null)
                return null;

            if (ri.RecipeId != recipeId)
                throw new InvalidOperationException("Given recipe ingredient does not belong to given recipe.");

            _dbContext.Remove(ri);
            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            var recipe = await (from r in _dbContext.Recipes.Include(r2 => r2.RecipeIngredients)
                                                            .ThenInclude(r2 => r2.Ingredient)
                                where r.Id == ri.RecipeId
                                select r).SingleAsync(token).ConfigureAwait(false);

            return Models.Recipe.FromEntity(recipe, true);
        }

        public async Task<Models.Recipe> UpdateIngredientAsync(int recipeId, int recipeIngredientId, int ingredientId, double amount, string unit, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var ri = await _dbContext.FindAsync<RecipeIngredient>(new object[] { recipeIngredientId }, cancellationToken: token).ConfigureAwait(false);
            //var ri = await (from ingredient in _dbContext.RecipeIngredients
            //                where ingredient.RecipeId == recipeId &&
            //                      ingredient.IngredientId == ingredientId
            //                select ingredient).SingleAsync(token).ConfigureAwait(false);

            if (ri == null)
                return null;

            if (ri.RecipeId != recipeId)
                throw new InvalidOperationException("Given recipe ingredient does not belong to given recipe.");

            if (amount > 0.0)
                ri.Amount = amount;

            if (!string.IsNullOrWhiteSpace(unit))
                ri.Unit = unit;

            if (ingredientId != ri.IngredientId)
            {
                var targetIngredient = await _dbContext.FindAsync<Ingredient>(new object[] { ingredientId }, cancellationToken: token).ConfigureAwait(false);
                if (targetIngredient == null)
                {
                    throw new InvalidOperationException("Updated ingredient not found");
                }
                ri.IngredientId = ingredientId;
            }

            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);

            var recipe = await (from r in _dbContext.Recipes.Include(r2 => r2.RecipeIngredients)
                                                            .ThenInclude(r2 => r2.Ingredient)
                                where r.Id == ri.RecipeId
                                select r).SingleAsync(token).ConfigureAwait(false);

            return Models.Recipe.FromEntity(recipe, true);
        }
    }
}
