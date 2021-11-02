using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using RecipeAPI.Clients;
using Newtonsoft.Json.Linq;
using System.Text;

namespace RecipeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : Controller
    {
        HttpClient _recipeClient;
        public IConfiguration Configuration;

        public RecipeController(RecipeService recipeService, IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            this._recipeClient = recipeService.Client;
        }
        [HttpGet]
        public async Task<ContentResult> Get(string ingredients, int? number, int? offset, string type, string instructionsRequired)
        {
            string path = "/recipes/complexSearch";
            StringBuilder builder = new StringBuilder();
            builder.Append("?");
            if (!string.IsNullOrEmpty(ingredients))
            {
                builder.Append($"ingredients={ingredients}");
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }
                builder.Append($"type={type}");
            }

            if (!string.IsNullOrEmpty(instructionsRequired))
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }
                builder.Append($"instructionsRequired={instructionsRequired}");
            }

            if (number != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }
                builder.Append($"number={number}");
            }

            if (offset != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }
                builder.Append($"offset={offset}");
            }


            string auth = builder.Length > 0 ? "&" : string.Empty;
            auth += $"apiKey={Configuration["Spoonacular"]}";
            string url = path + builder.ToString() + auth;
            using var httpResponse = await _recipeClient.GetAsync(path + builder.ToString() + auth);

            httpResponse.EnsureSuccessStatusCode();
            string content = await httpResponse.Content.ReadAsStringAsync();

            return this.Content(content, "application/json");
        }

        [HttpGet]
        [Route("{id}/information")]
        public async Task<ContentResult> GetRecipe(int id)
        {
            string path = $"/recipes/{id}/information?apiKey={Configuration["Spoonacular"]}";
            using var httpResponse = await _recipeClient.GetAsync(path);

            httpResponse.EnsureSuccessStatusCode();
            string content = await httpResponse.Content.ReadAsStringAsync();

            return this.Content(content, "application/json"); ;
        }

        [HttpGet]
        [Route("{id}/analyzedInstructions")]
        public async Task<ContentResult> GetRecipeInstructions(int id)
        {
            string path = $"/recipes/{id}/analyzedInstructions?apiKey={Configuration["Spoonacular"]}";
            using var httpResponse = await _recipeClient.GetAsync(path);

            httpResponse.EnsureSuccessStatusCode();
            string content = await httpResponse.Content.ReadAsStringAsync();

            return this.Content(content, "application/json"); ;
        }

        [HttpGet]
        [Route("winePairing")]
        public async Task<ContentResult> GetRecipeInstructions(string wine)
        {
            string path = $"/food/wine/dishes?wine={wine}&apiKey={Configuration["Spoonacular"]}";
            using var httpResponse = await _recipeClient.GetAsync(path);

            httpResponse.EnsureSuccessStatusCode();
            string content = await httpResponse.Content.ReadAsStringAsync();

            return this.Content(content, "application/json"); ;
        }
    }
}
