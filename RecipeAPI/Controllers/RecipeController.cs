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
        public async Task<ContentResult> Get()
        {
            string path = "/recipes/findByIngredients";
            string query = "?ingredients=apples,+flour,+sugar&number=2";
            string auth = $"&apiKey={Configuration["Spoonacular"]}";
            using var httpResponse = await _recipeClient.GetAsync(path + query + auth);

            httpResponse.EnsureSuccessStatusCode();
            string content = await httpResponse.Content.ReadAsStringAsync();

            return this.Content(content, "application/json"); ;
        }
    }
}
