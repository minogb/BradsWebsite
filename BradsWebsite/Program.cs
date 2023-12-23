using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using BradsWebsite.Areas.Post;
using BradsWebsite.Authentication;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
CookieAuthenticationDefaults.AuthenticationScheme
).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
builder.Services.AddMvc();
/*todo
builder.Services.AddTransient(
    m => new UserManager(
        Configuration
            .GetValue<string>(
                "ConnectionString" //this is a string constant
            )
        )
    );*/
builder.Services.AddTransient(m => new UserManager(builder.Configuration));

builder.Services.AddTransient(m => new PostManager(builder.Configuration));

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Post",
    areaName: "Post",
        defaults: new { controller = "Post", action = "Index" },
        pattern: "Post/{action=Index}/{id?}"
    );
    endpoints.MapAreaControllerRoute(
        name: "Pyramid",
    areaName: "Pyramid",
        defaults: new { controller = "Pyramid", action = "Index" },
        pattern: "Pyramid/{action=Index}/{id?}"
    );
    endpoints.MapAreaControllerRoute(
        name: "Resource",
    areaName: "Resource",
        defaults: new { controller = "Resource", action = "Index" },
        pattern: "Resource/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

});
app.Run();
