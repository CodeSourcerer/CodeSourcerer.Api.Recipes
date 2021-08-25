using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeSourcerer.Api.Recipes.Models;
using CodeSourcerer.Api.Recipes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeSourcerer.Api.Recipes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IRecipeService _recipeSvc;

        public RecipeController(ILogger<RecipeController> logger, IRecipeService recipeSvc)
        {
            _logger = logger;
            _recipeSvc = recipeSvc;
        }

        [HttpPut]
        [Produces("application/json", Type = typeof(Recipe))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<Recipe>> NewRecipe([FromBody] Recipe recipe, CancellationToken token = default)
        {
            try
            {
                var addedRecipe = await _recipeSvc.AddAsync(recipe, token).ConfigureAwait(false);

                return Ok(addedRecipe);
            }
            catch (Exception ex)
            {
                var problem = new ProblemDetails
                {
                    Title = "Error Creating Recipe",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json", Type = typeof(Recipe))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<Recipe>> GetById(int id, CancellationToken token = default)
        {
            try
            {
                var recipe = await _recipeSvc.GetAsync(id, token).ConfigureAwait(false);

                if (recipe == null)
                {
                    return NotFound();
                }

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                var problem = new ProblemDetails
                {
                    Title = "Error Creating Recipe",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        [HttpGet("test")]
        public ActionResult<IEnumerable<Recipe>> GetTest()
        {
            _logger.LogInformation("GetTest()");

            Recipe[] recipes = {
                new Recipe
                {
                    Id = 1,
                    Name = "Steak",
                    Notes = "Misc notes",
                    Servings = 1,
                    Tags = "Red Meat, Grilling, Dinners"
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Fried Chicken",
                    Notes = "Don't use an air fryer, you embecile.",
                    Servings = 2,
                    Tags = "Chicken, Dinners"
                }
            };

            return Ok(recipes);
        }
    }
}
