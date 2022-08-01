using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Contracts.Responses;
using RestingMovies.Api.Mappings;
using RestingMovies.Api.Repositories;

namespace RestingMovies.Api.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task<MovieResponse> Create(CreateMovieRequest createMovieRequest)
    {
        var movie = createMovieRequest.ToMovie();
        await _movieRepository.SaveMovie(movie);
        return movie.ToMovieResponse();
    }

    public async Task<IList<MovieResponse>> GetAll(string? name)
    {
        var movies = name is null ? await _movieRepository.GetAllMovies() : await _movieRepository.GetMoviesByName(name);
        return movies.Select(x => x.ToMovieResponse()).ToList();
    }

    public async Task<MovieResponse?> GetById(int id)
    {
        var movie = await _movieRepository.GetMovieById(id);
        return movie?.ToMovieResponse();
    }

    public async Task<bool> DeleteById(int id)
    {
        var movieToDelete = await _movieRepository.GetMovieById(id);
        if (movieToDelete is null) return false;
        await _movieRepository.DeleteMovie(movieToDelete);
        return true;
    }
}