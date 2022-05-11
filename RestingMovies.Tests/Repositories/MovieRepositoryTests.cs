using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;
using FluentAssertions;

namespace RestingMovies.Tests.Repositories;

[TestClass]
public class MovieRepositoryTests
{
    private readonly DbContextOptions<RestingMoviesDbContext> _dbContextOptions;

    public MovieRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<RestingMoviesDbContext>()
            .UseInMemoryDatabase("RestingMovies_Tests")
            .Options;
    }

    private IMovieRepository GetMovieRepository(RestingMoviesDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        return new MovieRepository(dbContext);
    }

    [TestMethod]
    public async Task MovieRepository_ShouldSaveMovie()
    {
        var moviesDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetMovieRepository(moviesDbContext);

        var movie = new Movie
        {
            Name = "Saving Private Ryan",
            Director = "Steven Spielberg",
            Genre = "War",
            ReleaseYear = 1998
        };

        await repository.SaveMovie(movie);

        movie.Name.Should().Be("Saving Private Ryan");
        movie.Director.Should().Be("Steven Spielberg");
        movie.Genre.Should().Be("War");
        movie.ReleaseYear.Should().Be(1998);
    }

    [TestMethod]
    public async Task MovieRepository_ShouldGetById()
    {
        var moviesDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetMovieRepository(moviesDbContext);

        var movie = new Movie
        {
            Name = "Saving Private Ryan",
            Director = "Steven Spielberg",
            Genre = "War",
            ReleaseYear = 1998
        };

        await moviesDbContext.Movies.AddAsync(movie);
        await moviesDbContext.SaveChangesAsync();

        var foundMovie = await repository.GetMovieById(movie.Id);

        foundMovie.Should().NotBeNull();
        foundMovie?.Id.Should().Be(movie.Id);
    }

    [TestMethod]
    public async Task MovieRepository_ShouldGetAllMovies()
    {
        var moviesDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetMovieRepository(moviesDbContext);

        var movies = new List<Movie>
        {
            new() { Name = "First", Director = "Myself", Genre = "Action", ReleaseYear = 2022 },
            new() { Name = "Second", Director = "Myself", Genre = "Action", ReleaseYear = 2023 }
        };

        await moviesDbContext.Movies.AddRangeAsync(movies);
        await moviesDbContext.SaveChangesAsync();

        var moviesFromDb = await repository.GetAllMovies();

        moviesFromDb.ToList().Count.Should().Be(2);
    }

    [TestMethod]
    public async Task MovieRepository_ShouldGetAllMoviesByName()
    {
        var moviesDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetMovieRepository(moviesDbContext);

        var movies = new List<Movie>
        {
            new() { Name = "First", Director = "Myself", Genre = "Action", ReleaseYear = 2022 },
            new() { Name = "Second", Director = "Myself", Genre = "Action", ReleaseYear = 2023 }
        };

        await moviesDbContext.Movies.AddRangeAsync(movies);
        await moviesDbContext.SaveChangesAsync();

        var moviesFromDb = (await repository.GetMoviesByName("first")).ToList();
        var foundMovie = moviesFromDb[0];
        
        moviesFromDb.Count.Should().Be(1);
        foundMovie.Should().NotBeNull();
        foundMovie.Name.Should().Be("First");
    }

    [TestMethod]
    public async Task MovieRepository_ShouldDeleteMovie()
    {
        var moviesDbContext = new RestingMoviesDbContext(_dbContextOptions);
        var repository = GetMovieRepository(moviesDbContext);

        var movie = new Movie
        {
            Name = "Saving Private Ryan",
            Director = "Steven Spielberg",
            Genre = "War",
            ReleaseYear = 1998
        };

        await moviesDbContext.Movies.AddAsync(movie);
        await moviesDbContext.SaveChangesAsync();

        await repository.DeleteMovie(movie);

        var count = await moviesDbContext.Movies.CountAsync();

        count.Should().Be(0);
    }
}