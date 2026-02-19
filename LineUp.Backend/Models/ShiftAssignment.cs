using System.Text.Json.Serialization;

namespace LineUp.Backend.Models;

public class ShiftAssignment
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? DayOfWeek { get; set; } // Optional: "Monday", "Tuesday", etc.

    // Navigation property
    [JsonIgnore]
    public Availability Availability { get; set; } = null!;

    [JsonIgnore]
    public Schedule Schedule { get; set; } = null!;
}
