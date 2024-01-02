


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SchiftPlanner;
using SchiftPlanner.Models;
using SchiftPlanner.Services;
using SchiftPlanner.Services.Interfaces;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DatabaseContext>(builder =>
{
    builder.UseSqlServer(@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=SchiftDatabase;Integrated Security=True");
});


builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 2;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;


}).AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "841855959515-rddhhri98coao5h632orn9qc8p13nj4o.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";

});

builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
{
        facebookOptions.AppId = "1269870073678248";
        facebookOptions.AppSecret = "616500424ca440613a0fae04b5d06dac";
});


builder.Services.AddAuthentication().AddMicrosoftAccount(MicrosoftAccountOptions =>
{
    MicrosoftAccountOptions.ClientId = "5069a503-4a4f-4b82-8b64-d88b8e800406"; 
    MicrosoftAccountOptions.ClientSecret = "bBv8Q~NJblKdukz5R.dnGifk2gdqBSgtFnHE5bco";
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.SlidingExpiration = true;
});



builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISubsServices, SubsServices>();
builder.Services.AddScoped<IOpinionServices, OpinionServices>();
builder.Services.AddScoped<ICustomer_TimeTableServices, Customer_TimeTableServices>();
builder.Services.AddScoped<IDay_CustomerServicesFirstGenerate, Day_CustomerServices>();
builder.Services.AddScoped<IDay_Customer_EditServices, Day_CustomerServices>();
builder.Services.AddScoped<IDay_Term, Day_CustomerServices>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();




app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();


