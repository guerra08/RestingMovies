using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestingMovies.Api.Entities;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;

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

        Assert.AreEqual("Saving Private Ryan", movie.Name);
        Assert.AreEqual("Steven Spielberg", movie.Director);
        Assert.AreEqual("War", movie.Genre);
        Assert.AreEqual(1998, movie.ReleaseYear);
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

        Assert.IsNotNull(foundMovie);
        Assert.AreEqual(movie.Id, foundMovie.Id);
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

        Assert.AreEqual(2, moviesFromDb.ToList().Count);
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

        Assert.AreEqual(1, moviesFromDb.Count);
        Assert.IsNotNull(foundMovie);
        Assert.AreEqual("First", foundMovie.Name);
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

        Assert.AreEqual(0, count);
    }
}