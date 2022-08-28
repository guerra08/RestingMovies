using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;
using RestingMovies.Api.Services;
using RestingMovies.Api.Validation;

namespace RestingMovies.Api.Endpoints;

public class RatingEndpoints : IEndpointConfiguration
{
    public void AddServices(IServiceCollection services)
    {
        services.AddDbContext<RestingMoviesDbContext>(
            c =>
            {
                c.UseSqlite("Data Source=restingmovies.db");
            });
        services.AddScoped<IValidator<CreateRatingRequest>, CreateRatingRequestValidator>();
        services.AddTransient<IRatingRepository, RatingRepository>();
        services.AddTransient<IRatingService, RatingService>();
    }

    public void MapEndpoints(WebApplication app)
    {
        app.MapPost("/ratings", HandlePostRating)
            .WithName("CreateRating");

        app.MapGet("/ratings", HandleGetRatings)
            .WithName("GetRatings");

        app.MapGet("/ratings/movie/{id}", HandleGetRatingsOfMovie)
            .WithName("GetRatingsOfMovie");

        app.MapGet("/ratings/{id}", HandleGetRatingById);
    }

    internal async Task<IResult> HandlePostRating(
        IRatingService ratingService,
        IValidator<CreateRatingRequest> validator,
        CreateRatingRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var response = await ratingService.Create(request);
        return Results.Created($"/rating/{response.Id}", response);
    }

    internal async Task<IResult> HandleGetRatings(IRatingService ratingService)
    {
        var ratings = await ratingService.GetAll();
        return Results.Ok(ratings);
    }

    internal async Task<IResult> HandleGetRatingsOfMovie(IRatingService ratingService, int movieId)
    {
        var ratings = await ratingService.GetByMovieId(movieId);
        return Results.Ok(ratings);
    }

    internal async Task<IResult> HandleGetRatingById(IRatingService ratingService, int ratingId)
    {
        return await ratingService.GetById(ratingId) switch
        {
            { } ratingResponse => Results.Ok(ratingResponse),
            null => Results.NotFound()
        };
    }
}