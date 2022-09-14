using System.Threading.Tasks;
using Bogus;
using Moq;
using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Repositories;
using RestingMovies.Api.Services;

namespace RestingMovies.Tests.Services;

public class RatingServiceTests
{
    private readonly Faker<CreateRatingRequest> _createRatingRequestFaker;
    private readonly Mock<IRatingRepository> _ratingRepository;
    private readonly IRatingService _sut;

    public RatingServiceTests()
    {
        _ratingRepository = new Mock<IRatingRepository>();
        _sut = new RatingService(_ratingRepository.Object);
        _createRatingRequestFaker = new Faker<CreateRatingRequest>();
        _createRatingRequestFaker
            .RuleFor(x => x.MovieId, 1)
            .RuleFor(x => x.Text, f => f.Lorem.Sentence())
            .RuleFor(x => x.Score, 5);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedRatingResponse()
    {
        var request = _createRatingRequestFaker.Generate();

        _ratingRepository
            .Setup(X => X.SaveRating(It.IsAny<Rating>()))
            .Returns(Task.CompletedTask);

        var result = await _sut.Create(request);
        
        _ratingRepository.Verify(x => x.SaveRating(It.IsAny<Rating>()));
        result.Score.Should().Be(request.Score);
    }
}