using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Contracts.Responses;

namespace RestingMovies.Api.Services;

public interface IMovieService
{
    Task<MovieResponse> Create(CreateMovieRequest createMovieRequest);
    Task<IList<MovieResponse>> GetAll(string? name);
    Task<MovieResponse?> GetById(int id);
    Task<bool> DeleteById(int id);
}