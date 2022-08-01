using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Contracts.Responses;
using RestingMovies.Api.Mappings;
using RestingMovies.Api.Repositories;

namespace RestingMovies.Api.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;

    public RatingService(IRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    public async Task<RatingResponse> Create(CreateRatingRequest createRatingRequest)
    {
        var rating = createRatingRequest.ToRating();
        await _ratingRepository.SaveRating(rating);
        return rating.ToRatingResponse();
    }

    public async Task<IList<RatingResponse>> GetAll()
    {
        var ratings = await _ratingRepository.GetAllRatings();
        return ratings.Select(x => x.ToRatingResponse()).ToList();
    }

    public async Task<RatingResponse?> GetById(int id)
    {
        var rating = await _ratingRepository.GetRatingById(id);
        return rating?.ToRatingResponse();
    }

    public async Task<IList<RatingResponse>> GetByMovieId(int id)
    {
        var ratings = await _ratingRepository.GetRatingsByMovieId(id);
        return ratings.Select(x => x.ToRatingResponse()).ToList();
    }

    public async Task<bool> DeleteById(int id)
    {
        var ratingToDelete = await _ratingRepository.GetRatingById(id);
        if (ratingToDelete is null) return false;
        await _ratingRepository.DeleteRating(ratingToDelete);
        return true;
    }
}