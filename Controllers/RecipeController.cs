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
                var msg = (ex is AggregateException exception) ? exception.InnerExceptions.First().Message : ex.Message;
                var problem = new ProblemDetails
                {
                    Title = "Error Creating Recipe",
                    Detail = msg,
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(problem.Status.Value, problem);
            }
        }

        [HttpPost]
        [Produces("application/json", Type = typeof(Recipe))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<Recipe>> UpdateRecipe([FromBody] Recipe recipe, CancellationToken token = default)
        {
            try
            {
                var updatedRecipe = await _recipeSvc.UpdateAsync(recipe, token).ConfigureAwait(false);

                if (updatedRecipe == null)
                {
                    var problem = new ProblemDetails
                    {
                        Title = "Error Updating Recipe",
                        Detail = "The recipe with the given Id could not be found.",
                        Status = StatusCodes.Status404NotFound
                    };

                    return StatusCode(problem.Status.Value, problem);
                }

                return Ok(updatedRecipe);
            }
            catch (Exception ex)
            {
                var msg = (ex is AggregateException exception) ? exception.InnerExceptions.First().Message : ex.Message;
                var problem = new ProblemDetails
                {
                    Title = "Error Updating Recipe",
                    Detail = msg,
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(problem.Status.Value, problem);
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
                var msg = (ex is AggregateException exception) ? exception.InnerExceptions.First().Message : ex.Message;
                var problem = new ProblemDetails
                {
                    Title = "Error Retrieving Recipe",
                    Detail = msg,
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(problem.Status.Value, problem);
            }
        }

        [HttpGet]
        [Produces("application/json", Type = typeof(Recipe))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<Recipe>> All(CancellationToken token = default)
        {
            try
            {
                var recipes = await _recipeSvc.GetAllAsync(token).ConfigureAwait(false);

                return Ok(recipes);
            }
            catch (Exception ex)
            {
                var msg = (ex is AggregateException exception) ? exception.InnerExceptions.First().Message : ex.Message;
                var problem = new ProblemDetails
                {
                    Title = "Error Retrieving Recipes",
                    Detail = msg,
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(problem.Status.Value, problem);
            }
        }

        [HttpPost("{recipeId}/Ingredients")]
        [Produces("application/json", Type = typeof(Recipe))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<Recipe>> AddIngredient(int recipeId, [FromBody] RecipeIngredientRequest ingredientRequest, CancellationToken token = default)
        {
            try
            {
                var recipe = await _recipeSvc.AddIngredientAsync(recipeId, ingredientRequest.IngredientId, ingredientRequest.Amount, ingredientRequest.Unit, token).ConfigureAwait(false);
                if (recipe == null)
                {
                    var problem = new ProblemDetails
                    {
                        Title = "Error Adding Ingredient to Recipe",
                        Detail = "Unable to locate given recipe or ingredient",
                        Status = StatusCodes.Status404NotFound
                    };
                    return StatusCode(problem.Status.Value, problem);
                }

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                var msg = (ex is AggregateException exception) ? exception.InnerExceptions.First().Message : ex.Message;
                var problem = new ProblemDetails
                {
                    Title = "Error Adding Ingredient to Recipe",
                    Detail = msg,
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(problem.Status.Value, problem);
            }
        }

        [HttpDelete("{recipeId}/Ingredients/{recipeIngredientId}")]
        [Produces("application/json", Type = typeof(Recipe))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<Recipe>> DeleteIngredient(int recipeId, int recipeIngredientId, CancellationToken token = default)
        {
            try
            {
                var recipe = await _recipeSvc.DeleteIngredientAsync(recipeId, recipeIngredientId, token).ConfigureAwait(false);

                if (recipe == null)
                {
                    var problem = new ProblemDetails
                    {
                        Title = "Error Removing Ingredient From Recipe",
                        Detail = "Unable to locate given recipe ingredient",
                        Status = StatusCodes.Status404NotFound
                    };
                    return StatusCode(problem.Status.Value, problem);
                }

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                var msg = (ex is AggregateException exception) ? exception.InnerExceptions.First().Message : ex.Message;
                var problem = new ProblemDetails
                {
                    Title = "Error Removing Ingredient from Recipe",
                    Detail = msg,
                    Status = StatusCodes.Status400BadRequest
                };

                return StatusCode(problem.Status.Value, problem);
            }
        }

        [HttpPut("{recipeId}/Ingredients/{recipeIngredientId}")]
        [Produces("application/json", Type = typeof(Recipe))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<Recipe>> UpdateIngredient(int recipeId, int recipeIngredientId, [FromBody] RecipeIngredientRequest request, CancellationToken token = default)
        {
            try
            {
                var recipe = await _recipeSvc.UpdateIngredientAsync(recipeId, recipeIngredientId, request.IngredientId, request.Amount, request.Unit, token).ConfigureAwait(false);
                if (recipe == null)
                {
                    var problem = new ProblemDetails
                    {
                        Title = "Error Updating Ingredient for Recipe",
                        Detail = "Unable to locate given recipe ingredient",
                        Status = StatusCodes.Status404NotFound
                    };
                    return StatusCode(problem.Status.Value, problem);
                }

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                var msg = (ex is AggregateException exception) ? exception.InnerExceptions.First().Message : ex.Message;
                var problem = new ProblemDetails
                {
                    Title = "Error Updating Ingredient for Recipe",
                    Detail = msg,
                    Status = StatusCodes.Status400BadRequest
                };

                return StatusCode(problem.Status.Value, problem);
            }
        }

        // TODO:
        // Add AddStep()
        // Add DeleteStep()
        // Add UpdateStep()

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
