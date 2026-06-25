using FluentEmail.Core;
using LineUp.Backend.EmailTemplates;
using LineUp.Core.Models;

namespace LineUp.Backend.Services;

public class MailKitService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendShiftAssignmentEmail(bool updated, Availability availability)
    {
        await fluentEmail
            .To(availability.UserEmail, availability.UserName)
            .UsingTemplateFromFile(
                "../EmailTemplates/ShiftAssignmentEmail.cshtml",
                new ShiftAssignmentEmail()
            )
            .SendAsync();
    }

    public Task SendAvailabilityConfirmationEmail(bool updated, Availability availability)
    {
        throw new NotImplementedException();
    }
}
