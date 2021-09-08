using System;
using DbEntities = CodeSourcerer.RecipeContext.RecipeContext;

namespace CodeSourcerer.Api.Recipes.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public Ingredient Ingredient { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }

        public DbEntities.RecipeIngredient ToEntity()
        {
            return ToEntity(this);
        }

        public static DbEntities.RecipeIngredient ToEntity(RecipeIngredient model)
        {
            if (model.Ingredient == null) throw new ArgumentException("Ingredient in model cannot be null.");

            var entity = new DbEntities.RecipeIngredient
            {
                Amount = model.Amount,
                Unit = model.Unit,
                RecipeId = model.RecipeId,
                IngredientId = model.Ingredient.Id
            };

            return entity;
        }

        public static RecipeIngredient FromEntity(DbEntities.RecipeIngredient entity)
        {
            var recipeIngredient = new RecipeIngredient
            {
                Id = entity.Id,
                RecipeId = entity.RecipeId,
                Amount = entity.Amount,
                Unit = entity.Unit
            };

            if (entity.Ingredient != null)
            {
                recipeIngredient.Ingredient = Ingredient.FromEntity(entity.Ingredient);
            }

            return recipeIngredient;
        }
    }
}