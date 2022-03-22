using Internify.Data;
using Internify.Data.Models;
using Internify.Services.Administrator;
using Internify.Services.Application;
using Internify.Services.Article;
using Internify.Services.Candidate;
using Internify.Services.CandidateUniversity;
using Internify.Services.Comment;
using Internify.Services.Company;
using Internify.Services.Country;
using Internify.Services.Internship;
using Internify.Services.Review;
using Internify.Services.Specialization;
using Internify.Services.University;
using Internify.Web.Common;
using Internify.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InternifyDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddDbContext<InternifyDbContext>(options => options.UseSqlServer(connectionString));

builder.Services
    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InternifyDbContext>();

builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    })
    .AddRazorRuntimeCompilation();

// typeof(Program) did not work.
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); -- worked for a while
// Oddly enough, after a few days of working adequately, AutoMapper decided to simply start throwing exceptions for missing maps out of the blue
// despite the fact that those exact mapping profiles are written and nothing has changed including dependencies.
// Solution: No AutoMapper :).
// Consider using Mapster.

builder.Services.AddMemoryCache();

builder.Services.AddTransient<ICandidateService, CandidateService>();
builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IUniversityService, UniversityService>();
builder.Services.AddTransient<ICandidateUniversityService, CandidateUniversityService>();
builder.Services.AddTransient<IInternshipService, InternshipService>();
builder.Services.AddTransient<IApplicationService, ApplicationService>();
builder.Services.AddTransient<IArticleService, ArticleService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<IAdministratorService, AdministratorService>();
builder.Services.AddTransient<ICountryService, CountryService>();
builder.Services.AddTransient<ISpecializationService, SpecializationService>();

// Singleton?
builder.Services.AddTransient<RoleChecker>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
});

var app = builder.Build();

//app.PrepareDatabase();

app.Use(async (context, next) =>
{
    var clientIP = context.Connection.RemoteIpAddress.ToString();

    var torIps = File.ReadAllText("wwwroot/tor/torbulkexitlist.txt");

    if (torIps.Contains(clientIP))
    {
        context.Response.StatusCode = 403;
        return;
    }

    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    //app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();

    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

//app.UseWebSockets();

app.UseCookiePolicy();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapRazorPages();

app.Run();
