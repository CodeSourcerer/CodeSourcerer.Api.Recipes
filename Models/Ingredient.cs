using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntities = CodeSourcerer.RecipeContext.RecipeContext;

namespace CodeSourcerer.Api.Recipes.Models
{
    public class Ingredient
    {
        public int    Id   { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }

        public static Ingredient FromEntity(DbEntities.Ingredient ingredient)
        {
            return new Ingredient
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Tags = ingredient.Tags,
            };
        }

        public DbEntities.Ingredient ToEntity(DbEntities.Ingredient exisitng)
        {
            exisitng.Name = Name;
            exisitng.Tags = Tags;

            return exisitng;
        }

        public static DbEntities.Ingredient ToEntity(Ingredient ingredient)
        {
            return new DbEntities.Ingredient
            {
                Name = ingredient.Name,
                Tags = ingredient.Tags,
            };
        }
    }
}
