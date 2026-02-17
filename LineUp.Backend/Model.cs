using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class LineUpContext : DbContext
{
    // public DbSet<Schedule> Schedules { get; set; }
    // public DbSet<Availability> Availabilities { get; set; }

    public string DbPath { get; }

    public LineUpContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "LineUp.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseNpgsql(
            @"Host=localhost;Username=postgres;Password=postgres;Database=mydatabase"
        );
}
