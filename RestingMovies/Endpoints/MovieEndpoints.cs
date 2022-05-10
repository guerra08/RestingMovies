using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;

namespace RestingMovies.Api.Endpoints;

public static class MovieEndpoints
{
    public static void AddMovieServices(this IServiceCollection services)
    {
        services.AddDbContext<MoviesDbContext>(c => c.UseInMemoryDatabase("RestingMovies"));
        services.AddTransient<IMovieRepository, MovieRepository>();
    }
    
    public static void MapMovieEndpoints(this WebApplication app)
    {
        app.MapPost("/movie", HandlePostMovie)
            .WithName("CreateMovie");

        app.MapGet("/movie", HandleGetMovies)
            .WithName("GetMovies");

        app.MapGet("/movie/{id}", HandleGetMovieById)
            .WithName("GetMovieById");

        app.MapDelete("/movie/{id}", HandleDeleteMovieById)
            .WithName("DeleteMovieById");
    }

    internal static async Task<IResult> HandlePostMovie(IMovieRepository movieRepository, Movie movie)
    {
        await movieRepository.SaveMovie(movie);
        return Results.Created($"/movie/{movie.Id}", movie);
    }

    internal static async Task<IResult> HandleGetMovies(IMovieRepository movieRepository, string? name)
    {
        var movies = name is null ? await movieRepository.GetAllMovies() : await movieRepository.GetMoviesByName(name);
        return Results.Ok(movies);
    }

    internal static async Task<IResult> HandleGetMovieById(IMovieRepository movieRepository, int id)
    {
        return await movieRepository.GetMovieById(id) switch
        {
            { } movie => Results.Ok(movie),
            null => Results.NotFound()
        };
    }

    internal static async Task<IResult> HandleDeleteMovieById(IMovieRepository movieRepository, int id)
    {
        var movie = await movieRepository.GetMovieById(id);
        if (movie is null) return Results.NotFound();
        await movieRepository.DeleteMovie(movie);
        return Results.NoContent();
    }
}