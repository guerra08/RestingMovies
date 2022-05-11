namespace RestingMovies.Api.Contracts.Requests;

public class CreateMovieRequest
{
    public string Name { get; init; } = default!;
    public string Director { get; init; } = default!;
    public string Genre { get; init; } = default!;
    public int ReleaseYear { get; init; } = default!;
}