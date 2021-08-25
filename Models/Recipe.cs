using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DbEntities = CodeSourcerer.RecipeContext.RecipeContext;

namespace CodeSourcerer.Api.Recipes.Models
{
    public class Recipe
    {
        public int      Id { get; set; }
        public string   Tags { get; set; }
        [Range(0, int.MaxValue)]
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

        public DbEntities.Recipe ToEntity(DbEntities.Recipe exisitng)
        {
            exisitng.Name     = Name;
            exisitng.Notes    = Notes;
            exisitng.Tags     = Tags;
            exisitng.Servings = Servings;

            return exisitng;
        }

        public static DbEntities.Recipe ToEntity(Recipe recipe)
        {
            return new DbEntities.Recipe
            {
                Name     = recipe.Name,
                Notes    = recipe.Notes,
                Tags     = recipe.Tags,
                Servings = recipe.Servings
            };
        }
    }
}
