namespace Core.Features.ApplicationUser.Commands.Requests;

public class ChangeUserPasswordCommand : IRequest<GenericBaseResponse<string>>
{
    public int Id { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
