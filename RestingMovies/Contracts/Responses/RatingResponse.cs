namespace RestingMovies.Api.Contracts.Responses;

public class RatingResponse
{
    public int Id { get; set; }
    public int Score { get; set; }
    public string Text { get; set; } = default!;
    public int MovieId { get; set; }
}