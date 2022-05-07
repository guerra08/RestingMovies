using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Repositories;

public interface IRatingRepository
{
    public Task SaveRating(Rating rating);
    public Task<List<Rating>> GetAllRatings();
    public Task<Rating?> GetRatingById(int ratingId);
    public Task<List<Rating>> GetRatingsByMovieId(int movieId);
    public Task DeleteRating(Rating rating);
}