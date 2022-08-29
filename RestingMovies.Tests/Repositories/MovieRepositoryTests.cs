using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;
using FluentAssertions;

namespace RestingMovies.Tests.Repositories;

public class MovieRepositoryTests
{
    private readonly RestingMoviesDbContext _moviesDbContext;
    private readonly IMovieRepository _sut;

    public MovieRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<RestingMoviesDbContext>()
            .UseInMemoryDatabase("RestingMovies_Tests")
            .Options;
        _moviesDbContext = new RestingMoviesDbContext(dbContextOptions);
        _moviesDbContext.Database.EnsureDeleted();
        _moviesDbContext.Database.EnsureCreated();
        _sut = new MovieRepository(_moviesDbContext);
    }

    [Fact]
    public async Task MovieRepository_ShouldSaveMovie()
    {
        var movie = new Movie
        {
            Name = "Saving Private Ryan",
            Director = "Steven Spielberg",
            Genre = "War",
            ReleaseYear = 1998
        };

        await _sut.SaveMovie(movie);

        movie.Name.Should().Be("Saving Private Ryan");
        movie.Director.Should().Be("Steven Spielberg");
        movie.Genre.Should().Be("War");
        movie.ReleaseYear.Should().Be(1998);
    }

    [Fact]
    public async Task MovieRepository_ShouldGetById()
    {
        var movie = new Movie
        {
            Name = "Saving Private Ryan",
            Director = "Steven Spielberg",
            Genre = "War",
            ReleaseYear = 1998
        };

        await _moviesDbContext.Movies.AddAsync(movie);
        await _moviesDbContext.SaveChangesAsync();

        var foundMovie = await _sut.GetMovieById(movie.Id);

        foundMovie?.Should().NotBeNull();
        foundMovie?.Id.Should().Be(movie.Id);
    }

    [Fact]
    public async Task MovieRepository_ShouldGetAllMovies()
    {
        var movies = new List<Movie>
        {
            new() { Name = "First", Director = "Myself", Genre = "Action", ReleaseYear = 2022 },
            new() { Name = "Second", Director = "Myself", Genre = "Action", ReleaseYear = 2023 }
        };

        await _moviesDbContext.Movies.AddRangeAsync(movies);
        await _moviesDbContext.SaveChangesAsync();

        var moviesFromDb = await _sut.GetAllMovies();

        moviesFromDb.ToList().Count.Should().Be(2);
    }

    [Fact]
    public async Task MovieRepository_ShouldGetAllMoviesByName()
    {
        var movies = new List<Movie>
        {
            new() { Name = "First", Director = "Myself", Genre = "Action", ReleaseYear = 2022 },
            new() { Name = "Second", Director = "Myself", Genre = "Action", ReleaseYear = 2023 }
        };

        await _moviesDbContext.Movies.AddRangeAsync(movies);
        await _moviesDbContext.SaveChangesAsync();

        var moviesFromDb = (await _sut.GetMoviesByName("first")).ToList();
        var foundMovie = moviesFromDb[0];
        
        moviesFromDb.Count.Should().Be(1);
        foundMovie.Should().NotBeNull();
        foundMovie.Name.Should().Be("First");
    }

    [Fact]
    public async Task MovieRepository_ShouldDeleteMovie()
    {
        var movie = new Movie
        {
            Name = "Saving Private Ryan",
            Director = "Steven Spielberg",
            Genre = "War",
            ReleaseYear = 1998
        };

        await _moviesDbContext.Movies.AddAsync(movie);
        await _moviesDbContext.SaveChangesAsync();

        await _sut.DeleteMovie(movie);

        var count = await _moviesDbContext.Movies.CountAsync();

        count.Should().Be(0);
    }
}