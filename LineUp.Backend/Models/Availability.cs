using System.Text.Json.Serialization;

namespace LineUp.Backend.Models;

public class Availability
{
    public int Id { get; set; }
    public DateTime[] AvailabilitySlots { get; set; } //See https://github.com/user-attachments/assets/37a07d97-902a-4195-b558-985008aa8912 for a visual on how this works.

    [JsonIgnore]
    public required Schedule Schedule { get; set; }

    public required string UserName { get; set; } //NOT a "username" in the traditional sense. This holds the real name of the user.

    public string? UserEmail { get; set; }

    public Preferences? Preferences { get; set; }
}
