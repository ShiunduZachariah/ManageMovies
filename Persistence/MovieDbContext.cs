using ManageMovies.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace ManageMovies.Persistence;

public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("app");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieDbContext).Assembly);
        
        modelBuilder.Entity<Movie>()
            .Property(m => m.LastModified)
            .HasDefaultValueSql("NOW()");
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseAsyncSeeding(async (context, _, ct) =>
            {
                var sampleMovie = await context.Set<Movie>()
                    .FirstOrDefaultAsync(b => b.Title == "Sonic The HedgeHog 3", ct);

                if (sampleMovie == null)
                {
                    sampleMovie = Movie.Create("Sonic The HedgeHog 3", "Fantasy",
                        new DateTimeOffset(new DateTime(2025, 1, 3), TimeSpan.Zero), 7);
                    sampleMovie.UpdateLastModified();

                    await context.Set<Movie>().AddAsync(sampleMovie, ct);
                    await context.SaveChangesAsync(ct);
                }
            })
            .UseSeeding((context, _) =>
                {
                    var sampleMovie = context.Set<Movie>()
                        .FirstOrDefault(b => b.Title == "Sonic The HedgeHog 3");

                    if (sampleMovie == null)
                    {
                        sampleMovie = Movie.Create("Sonic The HedgeHog 3", "Fantasy",
                            new DateTimeOffset(new DateTime(2025, 1, 3), TimeSpan.Zero), 7);
                        

                        context.Set<Movie>().Add(sampleMovie);
                        context.SaveChanges();
                    }
                }
            );
    }
}