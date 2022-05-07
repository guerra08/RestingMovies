using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;

namespace RestingMovies.Api.Persistence;

public class RatingsDbContext : DbContext
{
    public RatingsDbContext(DbContextOptions<RatingsDbContext> options) : base(options)
    {
    }

    public DbSet<Rating> Ratings { get; set; }
}