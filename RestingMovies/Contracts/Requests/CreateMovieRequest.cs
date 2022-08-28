namespace RestingMovies.Api.Contracts.Requests;

public record CreateMovieRequest
{
    public string Name { get; init; }
    public string Director { get; init; }
    public string Genre { get; init; }
    public int ReleaseYear { get; init; }
}