using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace LineUp.Backend.Models;

[Index(nameof(Guid))]
public class Availability
{
    public int Id { get; set; }
    public Guid Guid { get; init; } = Guid.NewGuid();

    public DateTime[] AvailabilitySlots { get; set; } = [];

    [MaxLength(64)]
    public required string UserName { get; set; } //NOT a "username" in the traditional sense. This holds the real name of the user.

    [MaxLength(256)]
    public string? UserEmail { get; set; }

    public AvailabilityPreferences? Preferences { get; set; }

    // Navigation properties, ignored in JSON to not loop forever
    [JsonIgnore]
    public required Schedule Schedule { get; set; }
}
