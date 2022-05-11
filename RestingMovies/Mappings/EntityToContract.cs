using RestingMovies.Api.Contracts.Responses;
using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Mappings;

public static class EntityToContract
{

    public static MovieResponse ToMovieResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Name = movie.Name,
            Director = movie.Director,
            Genre = movie.Genre,
            ReleaseYear = movie.ReleaseYear
        };

    }

    public static RatingResponse ToRatingResponse(this Rating rating)
    {
        return new RatingResponse
        {
            Id = rating.Id,
            Score = rating.Score,
            Text = rating.Text,
            MovieId = rating.MovieId
        };
    }
}