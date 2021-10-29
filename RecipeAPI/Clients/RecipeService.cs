using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeAPI.Clients
{
    public class RecipeService
    {
        public HttpClient Client { get; }

        public RecipeService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.spoonacular.com");

            Client = client;
        }
    }
}
