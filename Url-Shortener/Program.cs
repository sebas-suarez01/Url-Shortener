var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("api/short-url", async (string url) =>
{
    if (!Uri.TryCreate(url, UriKind.Absolute, out _))
    {
        return Results.BadRequest();
    }

    return Results.Ok();
});


app.UseHttpsRedirection();

app.Run();
