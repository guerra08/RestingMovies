using Xunit;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;
using FluentAssertions;

namespace RestingMovies.Tests.Repositories;

public class RatingRepositoryTests
{
    private readonly DbContextOptions<RestingMoviesDbContext> _dbContextOptions;

    public RatingRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<RestingMoviesDbContext>()
            .UseInMemoryDatabase("RestingMovies_Tests")
            .Options;
    }

    private IRatingRepository GetRatingRepository(RestingMoviesDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        return new RatingRepository(dbContext);
    }

    [Fact]
    public async Task RatingRepository_ShouldSaveRating()
    {
        var ratingsDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetRatingRepository(ratingsDbContext);

        var rating = new Rating { Score = 5, Text = "Very nice!", MovieId = 1 };

        await repository.SaveRating(rating);

        rating.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task RatingRepository_ShouldGetAllRatings()
    {
        var ratingsDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetRatingRepository(ratingsDbContext);

        var rating = new Rating { Score = 5, Text = "Very nice!", MovieId = 1 };

        await ratingsDbContext.AddAsync(rating);
        await ratingsDbContext.SaveChangesAsync();

        var ratings = (await repository.GetAllRatings()).ToList();
        
        ratings.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task RatingRepository_ShouldDeleteRating()
    {
        var ratingsDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetRatingRepository(ratingsDbContext);
        
        var rating = new Rating { Score = 5, Text = "An example!", MovieId = 1 };
        
        await ratingsDbContext.AddAsync(rating);
        await ratingsDbContext.SaveChangesAsync();
        
        await repository.DeleteRating(rating);

        var count = await ratingsDbContext.Ratings.CountAsync();

        count.Should().Be(0);
    }

    [Fact]
    public async Task RatingRepository_ShouldGetRatingsByMovieId()
    {
        var ratingsDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetRatingRepository(ratingsDbContext);
        
        var firstRating = new Rating { Score = 5, Text = "An example!", MovieId = 1 };
        var secondRating = new Rating { Score = 7, Text = "Nice!", MovieId = 2 };
        var thirdRating = new Rating { Score = 6, Text = "Meh!", MovieId = 2 };

        await ratingsDbContext.AddRangeAsync(firstRating, secondRating, thirdRating);
        await ratingsDbContext.SaveChangesAsync();

        var ratingsOfMovieId2 = await repository.GetRatingsByMovieId(2);

        foreach (var rating in ratingsOfMovieId2)
        {
            rating.MovieId.Should().Be(2);
        }
    }

    [Fact]
    public async Task RatingRepository_ShouldGetRatingById()
    {
        var ratingsDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetRatingRepository(ratingsDbContext);
        
        var rating = new Rating { Score = 5, Text = "An example!", MovieId = 1 };

        await ratingsDbContext.AddAsync(rating);
        await ratingsDbContext.SaveChangesAsync();

        var ratingFromDb = await repository.GetRatingById(rating.Id);

        ratingFromDb?.Should().NotBeNull();
        ratingFromDb?.Id.Should().Be(rating.Id);
    }
}