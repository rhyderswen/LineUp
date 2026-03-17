using System.ComponentModel.DataAnnotations;
<<<<<<<< HEAD:LineUp.Backend/Models/AvailabilityCreateDto.cs
using LineUp.Backend.Models.Forms;
========
using System.Text.Json.Serialization;
using LineUp.Core.Models.Forms;
using Microsoft.EntityFrameworkCore;
>>>>>>>> origin/er/or-tools-scheduler:LineUp.Core/Models/Availability.cs

namespace LineUp.Core.Models;

public class AvailabilityCreateDto
{
    public DateTime[] AvailabilitySlots { get; set; } = [];

    [MaxLength(64)]
    public required string UserName { get; set; } //NOT a "username" in the traditional sense. This holds the real name of the user.

    [MaxLength(256)]
    public string? UserEmail { get; set; }

    public AvailabilityPreferences? Preferences { get; set; }

    public ICollection<FormQuestionAnswer> FormAnswers { get; set; } =
        new List<FormQuestionAnswer>();
<<<<<<<< HEAD:LineUp.Backend/Models/AvailabilityCreateDto.cs
========

    // Navigation properties, ignored in JSON to not loop forever
    [JsonIgnore]
    public Schedule Schedule { get; set; }
>>>>>>>> origin/er/or-tools-scheduler:LineUp.Core/Models/Availability.cs
}
