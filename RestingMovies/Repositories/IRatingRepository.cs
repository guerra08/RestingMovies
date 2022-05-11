using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Repositories;

public interface IRatingRepository
{
    public Task<IEnumerable<Rating>> GetAllRatings();
    public Task<Rating?> GetRatingById(int ratingId);
    public Task<IEnumerable<Rating>> GetRatingsByMovieId(int movieId);
    public Task SaveRating(Rating rating);
    public Task DeleteRating(Rating rating);
}