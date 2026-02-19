using LineUp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace LineUp.Backend;

public class LineUpContext : DbContext
{
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Availability> Availabilities { get; set; }

    public LineUpContext(DbContextOptions<LineUpContext> options)
        : base(options) { }
}
