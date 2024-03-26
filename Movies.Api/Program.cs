using FluentValidation;
using MessagingContracts;
using Movies.Api.Contracts;
using Movies.Api.Contracts.Requests;
using Movies.Api.Movies;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Host.UseWolverine(x =>
{
    x.PublishAllMessages().ToRabbitExchange("movies-exc", exchange =>
    {
        exchange.ExchangeType = ExchangeType.Direct;
        exchange.BindQueue("movies-queue", "exchange2movies");
    });

    x.UseRabbitMq(c =>
    {
        c.HostName = "localhost";
    }).AutoProvision();
    // For use in cloud porposesses
    //x.PublishAllMessages().ToSqsQueue("movies-queue");

    //x.useAmazonSqsTransport().AutoProvision();

    //you can use this too x.UseFluentValidatotion();
});// if you want to publish to the cloud just remove WolverineFX and install wolverineFx.AmazonSqs

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapPost("movies", async (
    CreateMovieRequest request, IMovieService movieService) =>
{
    var movie = request.MatToMovie();
    var result = await movieService.CreateAsync(movie);
    return result.Match(
        _ => Results.CreatedAtRoute("GetMovie", new { idOrSlug = movie.Id }, movie.MapToResponse()),
        failed => Results.BadRequest(failed.MapToResponse()));
});

app.MapGet("movies/{idOrSlug}", async (
    string idOrSlug, IMovieService movieService) =>
{
    var result = Guid.TryParse(idOrSlug, out var id)
        ? await movieService.GetByIdAsync(id)
        : await movieService.GetBySlugAsync(idOrSlug);

    return result is not null ?
        Results.Ok(result.MapToResponse()) :
        Results.NotFound();
}).WithName("GetMovie");

app.MapGet("movies", async (IMovieService movieService) =>
{
    var movies = await movieService.GetAllAsync();
    var moviesResponse = movies.MapToResponse();
    return Results.Ok(moviesResponse);
});

app.MapPut("movies/{id:guid}", async (
    Guid id,
    UpdateMovieRequest request,
    IMovieService movieService) =>
{
    var movie = request.MapToMovie(id);
    var result = await movieService.UpdateAsync(movie);

    return result.Match(
        m => m is not null ? Results.Ok(m.MapToResponse()) : Results.NotFound(),
        failed => Results.BadRequest(failed.MapToResponse()));
});

app.MapDelete("movies/{id:guid}", async (Guid id, IMovieService movieService) =>
{
    var deleted = await movieService.DeleteByIdAsync(id);
    return deleted ? Results.Ok() : Results.NotFound();
});



app.Run();

