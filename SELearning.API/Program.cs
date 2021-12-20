using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using SELearning.API.Models;
using SELearning.Infrastructure.Credibility;
using SELearning.Infrastructure.User;
using SELearning.Core.Credibility;
using SELearning.Infrastructure.Authorization.Configuration;
using SELearning.Infrastructure.Authorization.Pipeline.Operations;
using SELearning.Infrastructure.Authorization.Rules;

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


builder.Services.AddDbContext<SELearningContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ISELearningContext, SELearningContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ISectionService, SectionManager>();

builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IContentService, ContentManager>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentManager>();

builder.Services.AddScoped<ICredibilityRepository, CredibilityRepository>();
builder.Services.AddScoped<ICredibilityService, CredibilityCalculator>();

builder.Services.AddPermissionAuthorization()
    .AddRule<UserCredibilityRule>()
    .AddResourceRule<UserCredibilityRule>()
    .AddResourceRule<AuthoredResourceRule>()
    .AddPermissionPipeline<CredibilityOperation>()
    .AddPermissionPipeline<ModeratorOperation>()
    .Build();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<SELearningContext>();

#endregion

var app = builder.Build();

#region Serve

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();

    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "SELearning.API V1"); });
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
