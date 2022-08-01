using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Contracts.Responses;

namespace RestingMovies.Api.Services;

public interface IRatingService
{
    Task<RatingResponse> Create(CreateRatingRequest createRatingRequest);
    Task<IList<RatingResponse>> GetAll();
    Task<RatingResponse?> GetById(int id);
    Task<IList<RatingResponse>> GetByMovieId(int id);
    Task<bool> DeleteById(int id);
}