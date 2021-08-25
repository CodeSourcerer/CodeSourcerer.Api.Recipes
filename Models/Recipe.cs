using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntities = CodeSourcerer.RecipeContext.RecipeContext;

namespace CodeSourcerer.Api.Recipes.Models
{
    public class Recipe
    {
        public int      Id { get; set; }
        public string   Tags { get; set; }
        public int?     Servings { get; set; }
        public string   Name { get; set; }
        public string   Notes { get; set; }

        public static Recipe FromEntity(DbEntities.Recipe recipe)
        {
            return new Recipe
            {
                Id       = recipe.Id,
                Name     = recipe.Name,
                Notes    = recipe.Notes,
                Tags     = recipe.Tags,
                Servings = recipe.Servings
            };
        }
    }
}
