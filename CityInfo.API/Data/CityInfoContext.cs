using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Data;

public class CityInfoContext : DbContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PointOfInterest> PointsOfInterests { get; set; } = null!;

    public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City("New York City") { Id = 1, Description = "The one with that big park" }
            , new City("Antwerp") { Id = 2, Description = "The one with the cathedral that was never finished." }
            , new City("Paris") { Id = 3, Description = "The one with that big tower" });

        modelBuilder.Entity<PointOfInterest>().HasData(
            new PointOfInterest("Central Park") { Id = 1, CityId = 1, Description = "The most visited urban park in United States" }
            , new PointOfInterest("Empire State Building") { Id = 2, CityId = 1, Description = "A 102-story skyscraper located in Midtown Manhattan." }
            , new PointOfInterest("Cathedral") { Id = 3, CityId = 2, Description = "A Cothic style cathedral." });

        base.OnModelCreating(modelBuilder);


    }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("connectionstring");
        base.OnConfiguring(optionsBuilder);
    }*/
}
