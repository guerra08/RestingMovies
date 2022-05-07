using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Persistence;

public class MoviesDbContext : DbContext
{
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
}