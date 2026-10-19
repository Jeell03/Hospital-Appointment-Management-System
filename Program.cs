
var builder = WebApplication.CreateBuilder(args); // Create the application builder.

// Register MVC controllers and Razor Views.
builder.Services.AddControllersWithViews();

// Enable session storage for selected specialization.
builder.Services.AddDistributedMemoryCache(); // Store session data in server memory.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Expire idle sessions after 20 minutes.
    options.Cookie.HttpOnly = true; // Prevent JavaScript from accessing the session cookie.
    options.Cookie.IsEssential = true; // Allow the session cookie for application functionality.
});

// Register in-memory caching for the doctor list.
builder.Services.AddMemoryCache();

// Register response caching for HTTP responses.
builder.Services.AddResponseCaching();

// Register HttpClient for asynchronous API requests.
builder.Services.AddHttpClient();

var app = builder.Build(); // Build the application.

// Configure error handling and HTTPS for non-development environments.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Serve static files such as CSS, JavaScript and images.
app.UseStaticFiles();

// Enable routing.
app.UseRouting();

// Enable the response caching middleware.
app.UseResponseCaching();

// Enable session before MVC endpoints execute.
app.UseSession();

// Configure the default MVC route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Doctors}/{action=Index}/{id?}");

// Start the web application.
app.Run();
