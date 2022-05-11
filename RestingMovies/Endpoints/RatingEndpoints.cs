using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Mappings;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;

namespace RestingMovies.Api.Endpoints;

public static class RatingEndpoints
{
    public static void AddRatingServices(this IServiceCollection services)
    {
        services.AddDbContext<RestingMoviesDbContext>(
            c =>
            {
                c.UseSqlite("Data Source=restingmovies.db");
            });
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
        CreateRatingRequest request)
    {
        var rating = request.ToRating();
        await ratingRepository.SaveRating(rating);
        return Results.Created($"/rating/{rating.Id}", rating.ToRatingResponse());
    }

    internal static async Task<IResult> HandleGetRatings(IRatingRepository ratingRepository)
    {
        var ratings = await ratingRepository.GetAllRatings();
        return Results.Ok(ratings.Select(x => x.ToRatingResponse()));
    }

    internal static async Task<IResult> HandleGetRatingsOfMovie(IRatingRepository ratingRepository, int movieId)
    {
        var ratings = await ratingRepository.GetRatingsByMovieId(movieId);
        return Results.Ok(ratings.Select(x => x.ToRatingResponse()));
    }
}