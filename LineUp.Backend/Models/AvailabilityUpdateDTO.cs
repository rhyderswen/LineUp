using System.ComponentModel.DataAnnotations;
using LineUp.Core.Models;
using LineUp.Core.Models.Forms;

namespace LineUp.Backend.Models;

/// <summary>
/// DTO for updating an availability.
/// </summary>
public class AvailabilityUpdateDto
{
    /// <summary>
    /// The ID of the availability to update.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The availability slots.
    /// </summary>
    public DateTime[] AvailabilitySlots { get; set; } = [];

    /// <summary>
    /// The user's name.
    /// </summary>
    [MaxLength(64)]
    public required string UserName { get; set; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    [MaxLength(256)]
    public string? UserEmail { get; set; }

    /// <summary>
    /// The user's availability preferences.
    /// </summary>
    public AvailabilityPreferences? Preferences { get; set; }

    /// <summary>
    /// The user's form answers.
    /// </summary>
    public ICollection<FormQuestionAnswer> FormAnswers { get; set; } =
        new List<FormQuestionAnswer>();
}
