using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;
using RestingMovies.Api.Services;
using RestingMovies.Api.Validation;

namespace RestingMovies.Api.Endpoints;

public class MovieEndpoints : IEndpointConfiguration
{
    public void AddServices(IServiceCollection services)
    {
        services.AddDbContext<RestingMoviesDbContext>(
            c => { c.UseSqlite("Data Source=restingmovies.db"); });
        services.AddScoped<IValidator<CreateMovieRequest>, CreateMovieRequestValidator>();
        services.AddTransient<IMovieRepository, MovieRepository>();
        services.AddTransient<IMovieService, MovieService>();
    }

    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("/movies", HandlePostMovie)
            .WithName("CreateMovie");

        app.MapGet("/movies", HandleGetMovies)
            .WithName("GetMovies");

        app.MapGet("/movies/{id}", HandleGetMovieById)
            .WithName("GetMovieById");

        app.MapDelete("/movies/{id}", HandleDeleteMovieById)
            .WithName("DeleteMovieById");
    }

    internal async Task<IResult> HandlePostMovie(
        IMovieService movieService,
        IValidator<CreateMovieRequest> validator,
        CreateMovieRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
        var movieResponse = await movieService.Create(request);
        return Results.Created($"/movie/{movieResponse.Id}", movieResponse);
    }

    internal async Task<IResult> HandleGetMovies(IMovieService movieService, string? name)
    {
        var movies = await movieService.GetAll(name);
        return Results.Ok(movies);
    }

    internal async Task<IResult> HandleGetMovieById(IMovieService movieService, int id)
    {
        return await movieService.GetById(id) switch
        {
            { } movieResponse => Results.Ok(movieResponse),
            null => Results.NotFound()
        };
    }

    internal async Task<IResult> HandleDeleteMovieById(IMovieService movieService, int id)
    {
        var deleted = await movieService.DeleteById(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}