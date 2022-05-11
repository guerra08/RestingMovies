using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Mappings;

public static class RequestsToEntities
{

    public static Movie ToMovie(this CreateMovieRequest req)
    {
        return new Movie
        {
            Name = req.Name,
            Director = req.Director,
            Genre = req.Genre,
            ReleaseYear = req.ReleaseYear
        };
    }

    public static Rating ToRating(this CreateRatingRequest req)
    {
        return new Rating
        {
            Score = req.Score,
            Text = req.Text,
            MovieId = req.MovieId
        };
    }
    
}