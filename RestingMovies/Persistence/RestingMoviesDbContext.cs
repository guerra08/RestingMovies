using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Persistence;

public class RestingMoviesDbContext : DbContext
{
    
    public RestingMoviesDbContext(DbContextOptions<RestingMoviesDbContext> options) : base(options)
    {
    }
    
    public DbSet<Movie> Movies { get; set; }
    
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Movie)
            .WithMany(m => m.Ratings)
            .HasForeignKey(r => r.MovieId);
    }
}