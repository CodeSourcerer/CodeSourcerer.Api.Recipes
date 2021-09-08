using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeSourcerer.Api.Recipes.Models
{
    public class AddIngredientRequest
    {
        public int IngredientId { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
    }
}
