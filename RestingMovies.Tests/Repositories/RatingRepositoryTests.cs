using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;

namespace RestingMovies.Tests.Repositories;

[TestClass]
public class RatingRepositoryTests
{
    private readonly DbContextOptions<RatingsDbContext> _dbContextOptions;

    public RatingRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<RatingsDbContext>()
            .UseInMemoryDatabase("RestingMovies_Tests")
            .Options;
    }

    private IRatingRepository GetRatingRepository(RatingsDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        return new RatingRepository(dbContext);
    }

    [TestMethod]
    public async Task RatingRepository_ShouldSaveRating()
    {
        var ratingsDbContext = new RatingsDbContext(_dbContextOptions);
        var repository = GetRatingRepository(ratingsDbContext);

        var rating = new Rating { Score = 5, Text = "Very nice!", MovieId = 1 };

        await repository.SaveRating(rating);

        Assert.IsTrue(rating.Id > 0);
    }
    
    [TestMethod]
    public async Task RatingRepository_ShouldGetAllRatings()
    {
        var ratingsDbContext = new RatingsDbContext(_dbContextOptions);
        var repository = GetRatingRepository(ratingsDbContext);

        var rating = new Rating { Score = 5, Text = "Very nice!", MovieId = 1 };

        await ratingsDbContext.AddAsync(rating);
        await ratingsDbContext.SaveChangesAsync();

        var ratings = (await repository.GetAllRatings()).ToList();

        Assert.IsTrue(ratings.Count > 0);
    }
}