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
    private readonly RestingMoviesDbContext _ratingsDbContext;
    private readonly IRatingRepository _sut;

    public RatingRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<RestingMoviesDbContext>()
            .UseInMemoryDatabase("RestingMovies_Tests")
            .Options;
        _ratingsDbContext = new RestingMoviesDbContext(dbContextOptions);
        _ratingsDbContext.Database.EnsureDeleted();
        _ratingsDbContext.Database.EnsureCreated();
        _sut = new RatingRepository(_ratingsDbContext);
    }

    [Fact]
    public async Task RatingRepository_ShouldSaveRating()
    {
        var rating = new Rating { Score = 5, Text = "Very nice!", MovieId = 1 };

        await _sut.SaveRating(rating);

        rating.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task RatingRepository_ShouldGetAllRatings()
    {
        var rating = new Rating { Score = 5, Text = "Very nice!", MovieId = 1 };

        await _ratingsDbContext.AddAsync(rating);
        await _ratingsDbContext.SaveChangesAsync();

        var ratings = (await _sut.GetAllRatings()).ToList();
        
        ratings.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task RatingRepository_ShouldDeleteRating()
    {
        var rating = new Rating { Score = 5, Text = "An example!", MovieId = 1 };
        
        await _ratingsDbContext.AddAsync(rating);
        await _ratingsDbContext.SaveChangesAsync();
        
        await _sut.DeleteRating(rating);

        var count = await _ratingsDbContext.Ratings.CountAsync();

        count.Should().Be(0);
    }

    [Fact]
    public async Task RatingRepository_ShouldGetRatingsByMovieId()
    {
        var firstRating = new Rating { Score = 5, Text = "An example!", MovieId = 1 };
        var secondRating = new Rating { Score = 7, Text = "Nice!", MovieId = 2 };
        var thirdRating = new Rating { Score = 6, Text = "Meh!", MovieId = 2 };

        await _ratingsDbContext.AddRangeAsync(firstRating, secondRating, thirdRating);
        await _ratingsDbContext.SaveChangesAsync();

        var ratingsOfMovieId2 = await _sut.GetRatingsByMovieId(2);

        foreach (var rating in ratingsOfMovieId2)
        {
            rating.MovieId.Should().Be(2);
        }
    }

    [Fact]
    public async Task RatingRepository_ShouldGetRatingById()
    {
        var rating = new Rating { Score = 5, Text = "An example!", MovieId = 1 };

        await _ratingsDbContext.AddAsync(rating);
        await _ratingsDbContext.SaveChangesAsync();

        var ratingFromDb = await _sut.GetRatingById(rating.Id);

        ratingFromDb?.Should().NotBeNull();
        ratingFromDb?.Id.Should().Be(rating.Id);
    }
}