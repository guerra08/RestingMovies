namespace RestingMovies.Api.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Director { get; set; }
    public string Genre { get; set; }
    public int ReleaseYear { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; }
}