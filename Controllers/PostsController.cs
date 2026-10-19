
using System.Net.Http.Json; // Import JSON response deserialization.
using Microsoft.AspNetCore.Mvc; // Import MVC controller features.

namespace Hospital.Controllers // Define the controller namespace.
{
    public class PostsController : Controller // Handle external post requests.
    {
        private readonly IHttpClientFactory _httpClientFactory; // Store the HTTP client factory.

        public PostsController(IHttpClientFactory httpClientFactory) // Receive the factory through dependency injection.
        {
            _httpClientFactory = httpClientFactory; // Save the factory.
        }

        public async Task<IActionResult> Index() // Fetch and display posts asynchronously.
        {
            var client = _httpClientFactory.CreateClient(); // Create an HTTP client.

            try // Handle API and network errors.
            {
                var posts = await client.GetFromJsonAsync<List<Post>>(
                    "https://jsonplaceholder.typicode.com/posts"); // Request and deserialize the posts.

                return View(posts ?? new List<Post>()); // Display the returned posts or an empty list.
            }
            catch (HttpRequestException) // Handle connection and HTTP request failures.
            {
                ViewBag.Error = "Unable to retrieve posts. Please try again later."; // Show an error message.
                return View(new List<Post>()); // Render the page without posts.
            }
        }
    }

    public class Post // Represent one API post.
    {
        public int Id { get; set; } // Store the post ID.
        public string Title { get; set; } = string.Empty; // Store the post title.
    }
}
