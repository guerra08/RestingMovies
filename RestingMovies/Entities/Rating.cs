using System.ComponentModel.DataAnnotations.Schema;

namespace RestingMovies.Api.Entities;

public class Rating
{
    public int Id { get; set; }
    public int Score { get; set; }
    public string Text { get; set; }
    public int MovieId { get; set; }
    public virtual Movie Movie { get; set; }
}