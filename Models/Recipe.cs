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
        public IList<RecipeIngredient> Ingredients { get; private set; }

        public Recipe()
        {
            Ingredients = new List<RecipeIngredient>();
        }

        public static Recipe FromEntity(DbEntities.Recipe entity, bool includeIngredients = false)
        {
            var recipe = new Recipe
            {
                Id       = entity.Id,
                Name     = entity.Name,
                Notes    = entity.Notes,
                Tags     = entity.Tags,
                Servings = entity.Servings
            };

            if (includeIngredients &&
                entity.RecipeIngredients?.Count > 0 && 
                entity.RecipeIngredients.First().Ingredient != null)
            {
                var ingredients = from ri in entity.RecipeIngredients
                                  select ri;
                foreach (var i in ingredients)
                {
                    recipe.Ingredients.Add(RecipeIngredient.FromEntity(i));
                }
            }

            return recipe;
        }

        public DbEntities.Recipe ToEntity(DbEntities.Recipe exisitng)
        {
            exisitng.Name     = Name;
            exisitng.Notes    = Notes;
            exisitng.Tags     = Tags;
            exisitng.Servings = Servings;

            return exisitng;
        }

        public static DbEntities.Recipe ToEntity(Recipe model)
        {
            return new DbEntities.Recipe
            {
                Name     = model.Name,
                Notes    = model.Notes,
                Tags     = model.Tags,
                Servings = model.Servings
            };
        }
    }
}
