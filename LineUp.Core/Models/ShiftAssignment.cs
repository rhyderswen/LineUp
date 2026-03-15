using System.Text.Json.Serialization;

namespace LineUp.Core.Models;

public class ShiftAssignment
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public Availability? Availability { get; set; }
}
