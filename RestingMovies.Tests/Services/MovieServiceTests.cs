using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Moq;
using RestingMovies.Api.Contracts.Requests;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Repositories;
using RestingMovies.Api.Services;

namespace RestingMovies.Tests.Services;

public class MovieServiceTests
{
    private readonly Faker<CreateMovieRequest> _createMovieRequestFaker;
    private readonly Faker<Movie> _movieFaker;
    private readonly Mock<IMovieRepository> _movieRepository;
    private readonly IMovieService _sut;

    public MovieServiceTests()
    {
        _movieRepository = new Mock<IMovieRepository>();
        _createMovieRequestFaker = new Faker<CreateMovieRequest>();
        _movieFaker = new Faker<Movie>();
        _sut = new MovieService(_movieRepository.Object);
        _movieFaker
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Director, f => f.Name.FullName())
            .RuleFor(x => x.Name, f => f.Lorem.Word())
            .RuleFor(x => x.Genre, f => f.Lorem.Word())
            .RuleFor(x => x.ReleaseYear, f => f.Date.PastDateOnly().Year);
        _createMovieRequestFaker
            .RuleFor(x => x.Director, f => f.Name.FullName())
            .RuleFor(x => x.Name, f => f.Lorem.Word())
            .RuleFor(x => x.Genre, f => f.Lorem.Word())
            .RuleFor(x => x.ReleaseYear, f => f.Date.PastDateOnly().Year);
    }

    [Fact]
    public async Task Create_ShouldCreateNewMovie()
    {
        var createMovieRequest = _createMovieRequestFaker.Generate();

        _movieRepository
            .Setup(x => x.SaveMovie(It.IsAny<Movie>()))
            .Returns(Task.CompletedTask);

        var result = await _sut.Create(createMovieRequest);

        _movieRepository.Verify(x => x.SaveMovie(It.IsAny<Movie>()));
        result.Name.Should().Be(createMovieRequest.Name);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllMovies()
    {
        IEnumerable<Movie> movies = new List<Movie>
        {
            _movieFaker.Generate(),
            _movieFaker.Generate()
        };

        _movieRepository.Setup(x => x.GetAllMovies())
            .Returns(Task.FromResult(movies));

        var result = await _sut.GetAll(null);

        _movieRepository.Verify(x => x.GetAllMovies());
        result.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetAllByName_ShouldReturnAllMovies()
    {
        IEnumerable<Movie> movies = new List<Movie>
        {
            _movieFaker.Generate()
        };

        var movieName = "A movie";

        _movieRepository.Setup(x => x.GetMoviesByName(movieName))
            .Returns(Task.FromResult(movies));

        var result = await _sut.GetAll(movieName);

        _movieRepository.Verify(x => x.GetMoviesByName(movieName));
        result.Count.Should().Be(1);
    }

    [Fact]
    public async Task GetById_ShouldReturnMovieResponseIfExists()
    {
        var movie = _movieFaker.Generate();
        var id = 1;

        _movieRepository.Setup(x => x.GetMovieById(id)).Returns(Task.FromResult<Movie?>(movie));

        var result = await _sut.GetById(id);

        result.Should().NotBeNull();
        result?.Name.Should().Be(movie.Name);
    }

    [Fact]
    public async Task GetById_ShouldReturnNullIfNotExists()
    {
        var id = 1;

        _movieRepository.Setup(x => x.GetMovieById(id)).Returns(Task.FromResult<Movie?>(null));

        var result = await _sut.GetById(id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteById_ShouldReturnTrueIfDeleted()
    {
        var movie = _movieFaker.Generate();
        var id = 1;

        _movieRepository.Setup(x => x.GetMovieById(id)).Returns(Task.FromResult<Movie?>(movie));

        var result = await _sut.DeleteById(id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteById_ShouldReturnFalseIfNotFound()
    {
        var id = 1;

        _movieRepository.Setup(x => x.GetMovieById(id)).Returns(Task.FromResult<Movie?>(null));

        var result = await _sut.DeleteById(id);

        result.Should().BeFalse();
    }
}