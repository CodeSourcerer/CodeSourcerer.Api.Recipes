using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeSourcerer.Api.Recipes.Models;

namespace CodeSourcerer.Api.Recipes.Services
{
    public interface IIngredientService
    {
        Task<Ingredient> AddAsync(Ingredient ingredient, CancellationToken token = default);
    }
}
