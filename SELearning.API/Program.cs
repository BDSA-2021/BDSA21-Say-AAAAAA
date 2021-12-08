using Microsoft.AspNetCore.Authentication.JwtBearer;
using SELearning.Core.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SELearning.API.Models;

var builder = WebApplication.CreateBuilder(args);
#region Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

string connectionString;

if (builder.Environment.IsDevelopment())
{

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "SELearning.API", Version = "v1" });
    });

    connectionString = builder.Configuration.GetConnectionString("SELearning");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("ProductionConnectionString");
}

builder.Services.AddDbContext<WeatherContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IWeatherContext, WeatherContext>();
builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

builder.Services.AddDbContext<CommentContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ICommentContext, CommentContext>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentManager>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<WeatherContext>()
    .AddDbContextCheck<CommentContext>();
#endregion

var app = builder.Build();
#region Serve
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "SELearning.API V1");
    });
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
    endpoints.MapHealthChecks("/healthz");
});

app.Migrate();
app.Run();
#endregion
