using ManageMovies.DTOs;
using ManageMovies.Models;
using ManageMovies.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ManageMovies.Services;

public class MovieService : IMovieService
{
    private readonly MovieDbContext _dbContext;
    private readonly ILogger<MovieService> _logger;

    public MovieService(MovieDbContext dbContext, ILogger<MovieService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    //Create Movie
    public async Task<MovieDto> CreateMovieAsync(CreateMovieDto command)
    {
        var movie = Movie.Create(command.Title, command.Genre, command.ReleaseDate, command.Rating);
        
        await _dbContext.Movies.AddAsync(movie);
        await _dbContext.SaveChangesAsync();
        
        return new MovieDto(movie.Id, movie.Title, movie.Genre, movie.ReleaseDate, movie.Rating);
    }

    //Get Movie By ID
    public async Task<MovieDto> GetMovieByIdAsync(Guid id)
    {
        var movie = await  _dbContext.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
            return null;
        
        return new MovieDto(movie.Id, movie.Title, movie.Genre, movie.ReleaseDate, movie.Rating);
    }

    //Get All Movies
    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        return await _dbContext.Movies
            .AsNoTracking()
            .Select(movie => new MovieDto(
                movie.Id,
                movie.Title,
                movie.Genre,
                movie.ReleaseDate,
                movie.Rating
                )).ToListAsync();
    }

    
    //Update Movies
    public async Task UpdateMovieAsync(Guid id, UpdateMovieDto command)
    {
        var movieToUpdate = await  _dbContext.Movies.FindAsync(id);
        if (movieToUpdate is null)
            throw new ArgumentNullException("Invalid Movie Id.", nameof(id));
        
        movieToUpdate.Update(command.Title, command.Genre, command.ReleaseDate, command.Rating);
        await  _dbContext.SaveChangesAsync();
    }

    //Patch Movies - partial update

    public async Task PatchMovieAsync(Guid id, PatchMovieDto command)
    {
        var movieToUpdate = await _dbContext.Movies.FindAsync(id);
        if (movieToUpdate is null)
            throw new ArgumentNullException(nameof(id), "Invalid Movie Id.");

        var any = command.Title is not null || command.Genre is not null || command.ReleaseDate is not null || command.Rating is not null;
        if (!any)
            throw new ArgumentException("Must supply at least one field to patch.");

        var title = command.Title ?? movieToUpdate.Title;
        var genre = command.Genre ?? movieToUpdate.Genre;
        var releaseDate = command.ReleaseDate ?? movieToUpdate.ReleaseDate;
        var rating = command.Rating ?? movieToUpdate.Rating;

        movieToUpdate.Update(title, genre, releaseDate, rating);

        await _dbContext.SaveChangesAsync();
    }


    
    //Delete Movies
    public async Task DeleteMovieAsync(Guid id)
    {
        var movieToDelete = await   _dbContext.Movies.FindAsync(id);

        if (movieToDelete != null)
        {
            _dbContext.Movies.Remove(movieToDelete);
            await  _dbContext.SaveChangesAsync();
        }
    }
}