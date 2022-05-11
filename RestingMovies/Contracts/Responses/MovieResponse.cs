namespace RestingMovies.Api.Contracts.Responses;

public class MovieResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Director { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public int ReleaseYear { get; set; }
}