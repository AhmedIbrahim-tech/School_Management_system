using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Services.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailsService _emailsService;
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IUrlHelper _urlHelper;
        #endregion

        #region Constructors
        public ApplicationUserService(UserManager<User> userManager,
                                      IHttpContextAccessor httpContextAccessor,
                                      IEmailsService emailsService,
                                      ApplicationDBContext applicationDBContext,
                                      IUrlHelper urlHelper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailsService = emailsService;
            _applicationDBContext = applicationDBContext;
            _urlHelper = urlHelper;
        }
        #endregion

        #region Handle Functions

        #region Create User
        public async Task<string> AddUserAsync(User user, string password)
        {
            var Transaction = await _applicationDBContext.Database.BeginTransactionAsync();
            try
            {
                var existUser = await _userManager.FindByEmailAsync(user.Email);

                if (existUser != null) return "EmailIsExist";

                var userByUserName = await _userManager.FindByNameAsync(user.UserName);

                if (userByUserName != null) return "UserNameIsExist";

                user.Password = password;

                var createResult = await _userManager.CreateAsync(user, password);

                if (!createResult.Succeeded)
                    return string.Join(",", createResult.Errors.Select(x => x.Description).ToList());

                var ListofUsers = await _userManager.Users.ToListAsync();

                await _userManager.AddToRoleAsync(user, "User");


                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var resquestAccessor = _httpContextAccessor.HttpContext.Request;
                var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host + _urlHelper.Action("ConfirmEmail", "Authentication",
                                      new { userId = user.Id, code = code });
                var message = $"To Confirm Email Click Link: <a href='{returnUrl}'></a>";

                var email = new EmailDto()
                {
                    MailTo = user.Email,
                    Body = message,
                    Subject = "ConFirm Email"
                };
                var sendemail = await _emailsService.SendEmailAsync(email);
                if (!sendemail) return "Failed";

                await Transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await Transaction.RollbackAsync();
                return "Failed";
            }

        }
        #endregion

        #endregion
    }
}
