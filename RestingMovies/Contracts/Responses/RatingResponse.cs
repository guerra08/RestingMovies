namespace RestingMovies.Api.Contracts.Responses;

public record RatingResponse
{
    public int Id { get; init; }
    public int Score { get; init; }
    public string Text { get; init; }
    public int MovieId { get; init; }
}