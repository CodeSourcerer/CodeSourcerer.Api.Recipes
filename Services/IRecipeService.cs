using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeSourcerer.Api.Recipes.Models;

namespace CodeSourcerer.Api.Recipes.Services
{
    public interface IRecipeService
    {
        Task<Recipe> AddAsync(Recipe recipe, CancellationToken token = default);
        Task<Recipe> GetAsync(int id, CancellationToken token = default);
        Task<Recipe> UpdateAsync(Recipe recipe, CancellationToken token = default);
        Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token = default);
        Task<Recipe> AddIngredientAsync(int recipeId, int ingredientId, double amount, string unit, CancellationToken token = default);
        Task<Recipe> DeleteIngredientAsync(int recipeId, int recipeIngredientId, CancellationToken token = default);
        Task<Recipe> UpdateIngredientAsync(int recipeId, int recipeIngredientId, int ingredientId, double amount, string unit, CancellationToken token = default);
    }
}
