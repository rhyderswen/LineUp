using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LineUp.Backend.EmailTemplates;

public class ShiftAssignmentEmail : PageModel
{
    public void OnGet() { }

    public string? UserName { get; set; }
    public string? ScheduleName { get; set; }
    public string? ShiftAssignmentsUl { get; set; }
}
