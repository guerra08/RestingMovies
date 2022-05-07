using System.ComponentModel.DataAnnotations;

namespace RestingMovies.Api.Entities;

public class Movie
{
    public int Id { get; set; }

    [Required] public string? Name { get; set; }

    [Required] public string? Director { get; set; }

    [Required] public string? Genre { get; set; }

    public int ReleaseYear { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; }
}