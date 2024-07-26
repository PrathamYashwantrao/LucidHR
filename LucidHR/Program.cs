using LucidHR.Data;
using LucidHR.Models;
using LucidHR.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using LucidHR.Controllers;
using DinkToPdf.Contracts;
using DinkToPdf;
using LucidHR.Services;

//--hasnain

var builder = WebApplication.CreateBuilder(args);

//------OfferLetter letter ----

builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();


builder.Services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));

//----offerlettrr ------
builder.Services.AddScoped<IOfferLetterApiClient, OfferLetterApiClient>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddTransient<PdfService>();
builder.Services.AddScoped<PayslipService>();


builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddTransient<PdfService>();

//----calender

builder.Services.AddScoped<HrmsRepo, HrmsServices>();

// Read email settings from configuration
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddSingleton(emailSettings);
builder.Services.AddTransient<EmailSendController>(provider => new EmailSendController(
    emailSettings.SmtpServer,
    emailSettings.SmtpPort,
    emailSettings.SmtpUsername,
    emailSettings.SmtpPassword,
    emailSettings.FromEmail
));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Crm}/{action=Index}/{id?}");

app.Run();
