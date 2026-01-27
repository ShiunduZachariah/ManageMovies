using ManageMovies.DTOs;
using ManageMovies.Services;

namespace ManageMovies.Endpoints;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this IEndpointRouteBuilder routes)
    {
        var movieApi = routes.MapGroup("/api/movies").WithTags("Movies");

        //Post a Movie
        movieApi.MapPost("/", async (IMovieService movieService, CreateMovieDto command) =>
        {
            var movie = await movieService.CreateMovieAsync(command);
            return TypedResults.Created($"/api/movies/{movie.Id}", movie);
        })
        .WithTags("CreateMovie")
        .WithSummary("Creates a new movie")
        .Produces<MovieDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
        
        //Get all Movies
        movieApi.MapGet("/", async (IMovieService movieService) =>
        {
            var movies = await  movieService.GetAllMoviesAsync();
            return TypedResults.Ok(movies);
        })
        .WithTags("GetAllMovies")
        .WithSummary("Gets all movies")
        .Produces<IEnumerable<MovieDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        
        //Get Movie By ID
        movieApi.MapGet("/{id}", async (IMovieService movieService, Guid id) =>
        {
            var movie = await movieService.GetMovieByIdAsync(id);
            return movie is null
                ? (IResult)TypedResults.NotFound(new {message = $"Movie with {id} not found!"}) 
                : TypedResults.Ok(movie);
        })
        .WithName("GetMovieById")
        .WithSummary("Gets a single movie by its ID")
        .Produces<MovieDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        
        //Update a Movie
        movieApi.MapPut("/{id}", async (IMovieService movieService, Guid id, UpdateMovieDto command) =>
        {
            await movieService.UpdateMovieAsync(id, command);
            return TypedResults.NoContent();
        })
        .WithName("UpdateMovie")
        .WithSummary("Updates a movie")
        .Produces(StatusCodes.Status204NoContent);
        
        //Delete a Movie
        movieApi.MapDelete("/{id}", async  (IMovieService movieService, Guid id) =>
        {
            await movieService.DeleteMovieAsync(id);        
            return  TypedResults.NoContent();
        })
        .WithTags("Movies")
        .WithSummary("Deletes a single movie by its ID")
        .Produces(StatusCodes.Status204NoContent);

    }
}