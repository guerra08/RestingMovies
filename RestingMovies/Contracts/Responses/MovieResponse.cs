namespace RestingMovies.Api.Contracts.Responses;

public record MovieResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Director { get; init; }
    public string Genre { get; init; }
    public int ReleaseYear { get; init; }
}