using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Mappings;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;

namespace RestingMovies.Api.Endpoints;

public static class MovieEndpoints
{
    public static void AddMovieServices(this IServiceCollection services)
    {
        services.AddDbContext<RestingMoviesDbContext>(
            c =>
            {
                c.UseSqlite("Data Source=restingmovies.db");
            });
        services.AddTransient<IMovieRepository, MovieRepository>();
    }
    
    public static void MapMovieEndpoints(this WebApplication app)
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

    internal static async Task<IResult> HandlePostMovie(IMovieRepository movieRepository, CreateMovieRequest request)
    {
        var movie = request.ToMovie();
        await movieRepository.SaveMovie(movie);
        return Results.Created($"/movie/{movie.Id}", movie.ToMovieResponse());
    }

    internal static async Task<IResult> HandleGetMovies(IMovieRepository movieRepository, string? name)
    {
        var movies = name is null ? await movieRepository.GetAllMovies() : await movieRepository.GetMoviesByName(name);
        return Results.Ok(movies.Select(m => m.ToMovieResponse()));
    }

    internal static async Task<IResult> HandleGetMovieById(IMovieRepository movieRepository, int id)
    {
        return await movieRepository.GetMovieById(id) switch
        {
            { } movie => Results.Ok(movie.ToMovieResponse()),
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