// Program.cs
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using BBURGERClone.Services;      // IProductService, ProductService
// using Stripe;                 // only if you enable Stripe features

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Configuration & logging
// -----------------------------
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

// optional: configure logging (defaults are usually fine)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// -----------------------------
// MVC + Razor pages
// -----------------------------
builder.Services.AddControllersWithViews();

// -----------------------------
// App services / DI
// -----------------------------
// Register your product service used by controllers (in-memory demo service)
builder.Services.AddSingleton<IProductService, ProductService>();

// Register IHttpContextAccessor if needed (used by layout for session/cart)
builder.Services.AddHttpContextAccessor();

// -----------------------------
// Session (for SessionCart)
// -----------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// -----------------------------
// Configure strongly-typed config sections if you want
// e.g. builder.Services.Configure<StripeOptions>(builder.Configuration.GetSection("Stripe"));
// -----------------------------

var app = builder.Build();

// -----------------------------
// Middleware pipeline
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); 

// Session MUST come before endpoints that need it
app.UseSession();

app.UseAuthorization();

// Default route for MVC controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// optional: map area routes if you use Areas
// app.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
