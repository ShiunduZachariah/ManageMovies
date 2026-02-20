using ManageMovies.DTOs;

namespace ManageMovies.Services;

public interface IMovieService
{
    Task<MovieDto> CreateMovieAsync(CreateMovieDto command);
    Task<MovieDto?> GetMovieByIdAsync(Guid id);
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task UpdateMovieAsync(Guid id, UpdateMovieDto command);
    Task PatchMovieAsync(Guid id, PatchMovieDto command);
    Task DeleteMovieAsync(Guid id);
}