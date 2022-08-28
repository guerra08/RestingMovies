namespace RestingMovies.Api.Contracts.Requests;

public record CreateRatingRequest
{
    public int Score { get; init; }
    public string Text { get; init; }
    public int MovieId { get; init; }
}