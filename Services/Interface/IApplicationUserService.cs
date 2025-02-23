namespace Services.Interface;

public interface IApplicationUserService
{
    Task<string> AddUserAsync(User user, string password);
}
