using CineFinder.API.Data;
using CineFinder.API.Repositories.Implementations;
using CineFinder.API.Repositories.Interfaces;
using CineFinder.API.Services.Implementations;
using CineFinder.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- Typed HttpClient → Java API (Spring Boot, porta 8080) ---
var javaBaseUrl = builder.Configuration["JavaApi:BaseUrl"] ?? "http://localhost:8080";
builder.Services.AddHttpClient<IJavaApiService, JavaApiService>(client =>
{
    client.BaseAddress = new Uri(javaBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
});

// --- DbContext (SQL Server) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Repositories ---
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<ISerieRepository, SerieRepository>();
builder.Services.AddScoped<IFavoritoRepository, FavoritoRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

// --- Services ---
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IFilmeService, FilmeService>();
builder.Services.AddScoped<ISerieService, SerieService>();
builder.Services.AddScoped<IFavoritoService, FavoritoService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// --- CORS para desenvolvimento ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "CineFinder API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseDefaultFiles();

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            ctx.Context.Response.Headers["Pragma"] = "no-cache";
            ctx.Context.Response.Headers["Expires"] = "0";
        }
    });
}
else
{
    app.UseStaticFiles();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
