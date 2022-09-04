using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;

namespace RestingMovies.Api.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly RestingMoviesDbContext _dbContext;

    public RatingRepository(RestingMoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Rating>> GetAllRatings()
    {
        return await _dbContext.Ratings.ToListAsync();
    }

    public async Task<Rating?> GetRatingById(int ratingId)
    {
        return await _dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == ratingId);
    }

    public async Task<IEnumerable<Rating>> GetRatingsByMovieId(int movieId)
    {
        return await _dbContext.Ratings.Where(x => x.MovieId == movieId).ToListAsync();
    }

    public async Task SaveRating(Rating rating)
    {
        await _dbContext.Ratings.AddAsync(rating);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteRating(Rating rating)
    {
        _dbContext.Ratings.Remove(rating);
        await _dbContext.SaveChangesAsync();
    }
}