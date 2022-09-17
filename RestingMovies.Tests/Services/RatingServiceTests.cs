using System.Collections.Generic;
using System.Linq;
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
    private readonly Faker<Rating> _ratingFaker;
    private readonly Mock<IRatingRepository> _ratingRepository;
    private readonly IRatingService _sut;

    public RatingServiceTests()
    {
        _ratingRepository = new Mock<IRatingRepository>();
        _sut = new RatingService(_ratingRepository.Object);
        _createRatingRequestFaker = new Faker<CreateRatingRequest>();
        _ratingFaker = new Faker<Rating>();
        _createRatingRequestFaker
            .RuleFor(x => x.MovieId, 1)
            .RuleFor(x => x.Text, f => f.Lorem.Sentence())
            .RuleFor(x => x.Score, 5);
        _ratingFaker
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Text, f => f.Lorem.Sentence())
            .RuleFor(x => x.Score, 5)
            .RuleFor(x => x.MovieId, 1);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedRatingResponse()
    {
        var request = _createRatingRequestFaker.Generate();

        _ratingRepository
            .Setup(x => x.SaveRating(It.IsAny<Rating>()))
            .Returns(Task.CompletedTask);

        var result = await _sut.Create(request);
        
        _ratingRepository.Verify(x => x.SaveRating(It.IsAny<Rating>()));
        result.Score.Should().Be(request.Score);
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfCurrentRatings()
    {
        IEnumerable<Rating> ratings = new List<Rating>
        {
            _ratingFaker.Generate(),
            _ratingFaker.Generate(),
            _ratingFaker.Generate()
        };

        _ratingRepository
            .Setup(x => x.GetAllRatings()).Returns(Task.FromResult(ratings));

        var result = (await _sut.GetAll()).ToList();

        _ratingRepository.Verify(x => x.GetAllRatings());

        result.Count.Should().Be(3);
    }

    [Fact]
    public async Task GetById_ShouldReturnRatingIfItExists()
    {

        var rating = _ratingFaker.Generate();

        _ratingRepository
            .Setup(x => x.GetRatingById(It.IsAny<int>())).Returns(Task.FromResult<Rating?>(rating));

        var result = await _sut.GetById(1);

        result.Should().NotBe(null);
        result.Text.Should().Be(rating.Text);
    }

    [Fact]
    public async Task GetById_ShouldReturnNullIfItDoesNotExists()
    {

        _ratingRepository
            .Setup(x => x.GetRatingById(It.IsAny<int>())).Returns(Task.FromResult<Rating?>(null));

        var result = await _sut.GetById(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByMovieId_ShouldReturnRatingsOfSpecificMovie()
    {

        IEnumerable<Rating> ratings = new List<Rating>
        {
            _ratingFaker.Generate(),
            _ratingFaker.Generate(),
            _ratingFaker.Generate()
        };

        _ratingRepository
            .Setup(x => x.GetRatingsByMovieId(It.IsAny<int>())).Returns(Task.FromResult(ratings));

        var result = (await _sut.GetByMovieId(1)).ToList();

        result.Count.Should().Be(3);
    }

    [Fact]
    public async Task DeleteById_ShouldReturnTrueIfDeleted()
    {

        var rating = _ratingFaker.Generate();
        var id = 1;

        _ratingRepository
            .Setup(x => x.GetRatingById(id)).Returns(Task.FromResult<Rating?>(rating));

        var result = await _sut.DeleteById(id);

        result.Should().Be(true);
    }
    
    [Fact]
    public async Task DeleteById_ShouldReturnFalseIfNotDeleted()
    {
        
        var id = 1;

        _ratingRepository
            .Setup(x => x.GetRatingById(id)).Returns(Task.FromResult<Rating?>(null));

        var result = await _sut.DeleteById(id);

        result.Should().Be(false);
    }
}