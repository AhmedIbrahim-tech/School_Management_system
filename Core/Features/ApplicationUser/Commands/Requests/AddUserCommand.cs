using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Features.ApplicationUser.Commands.Requests;

public class AddUserCommand : IRequest<GenericBaseResponse<string>>
{
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    [Compare("Password")]
    [NotMapped]
    public string PasswordConfirmed { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }

}
