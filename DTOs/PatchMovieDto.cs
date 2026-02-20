namespace ManageMovies.DTOs;

public record PatchMovieDto(string? Title, string? Genre, DateTimeOffset? ReleaseDate, double? Rating);

