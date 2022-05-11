namespace RestingMovies.Api.Contracts.Requests;

public class CreateRatingRequest
{
    public int Score { get; init; }
    public string Text { get; init; } = default!;
    public int MovieId { get; init; }
}