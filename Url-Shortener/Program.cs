using Microsoft.EntityFrameworkCore;
using Url_Shortener.Database;
using Url_Shortener.Database.Repository;
using Url_Shortener.Endpoints;
using Url_Shortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UrlShorteningService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<UrlRepository>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.ApplyMigrations();
}
app.UseCors();

app.AddEndpoints();

app.UseHttpsRedirection();

app.Run();
