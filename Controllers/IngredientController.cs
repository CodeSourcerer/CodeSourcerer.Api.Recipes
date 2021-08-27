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
    public class IngredientController : ControllerBase
    {
        private readonly ILogger<IngredientController> _logger;
        private readonly IIngredientService _ingredientSvc;

        public IngredientController(ILogger<IngredientController> logger, IIngredientService ingredientSvc)
        {
            _logger = logger;
            _ingredientSvc = ingredientSvc;
        }

        [HttpPut]
        [Produces("application/json", Type = typeof(Ingredient))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<ActionResult<Recipe>> NewIngredient([FromBody] Ingredient ingredient, CancellationToken token = default)
        {
            try
            {
                var addedIngredient = await _ingredientSvc.AddAsync(ingredient, token).ConfigureAwait(false);

                return Ok(addedIngredient);
            }
            catch (Exception ex)
            {
                var problem = new ProblemDetails
                {
                    Title = "Error Creating Ingredient",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(problem.Status.Value, problem);
            }
        }
    }
}
