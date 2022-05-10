using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;

namespace RestingMovies.Api.Endpoints;

public static class RatingEndpoints
{
    public static void AddRatingServices(this IServiceCollection services)
    {
        services.AddDbContext<RatingsDbContext>(c => c.UseInMemoryDatabase("RestingMovies"));
        services.AddTransient<IRatingRepository, RatingRepository>();
    }
    
    public static void MapRatingEndpoints(this WebApplication app)
    {
        app.MapPost("/ratings", HandlePostRating)
            .WithName("CreateRating");

        app.MapGet("/ratings", HandleGetRatings)
            .WithName("GetRatings");

        app.MapGet("/ratings/movie/{id}", HandleGetRatingsOfMovie)
            .WithName("GetRatingsOfMovie");
    }

    internal static async Task<IResult> HandlePostRating(IRatingRepository ratingRepository,
        Rating rating)
    {
        await ratingRepository.SaveRating(rating);
        return Results.Created($"/rating/{rating.Id}", rating);
    }

    internal static async Task<IResult> HandleGetRatings(IRatingRepository ratingRepository)
    {
        return Results.Ok(await ratingRepository.GetAllRatings());
    }

    internal static async Task<IResult> HandleGetRatingsOfMovie(IRatingRepository ratingRepository, int movieId)
    {
        return Results.Ok(await ratingRepository.GetRatingsByMovieId(movieId));
    }
}