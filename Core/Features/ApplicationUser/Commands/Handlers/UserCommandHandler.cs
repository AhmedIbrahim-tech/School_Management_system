using Microsoft.AspNetCore.Identity;

namespace Core.Features.ApplicationUser.Commands.Handlers;

public class UserCommandHandler : GenericBaseResponseHandler,
    IRequestHandler<AddUserCommand, GenericBaseResponse<string>>,
    IRequestHandler<EditUserCommand, GenericBaseResponse<string>>,
    IRequestHandler<DeleteUserCommand, GenericBaseResponse<string>>,
    IRequestHandler<ChangeUserPasswordCommand, GenericBaseResponse<string>>
{
    #region Fields
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _sharedResources;
    private readonly UserManager<User> _userManager;
    private readonly IApplicationUserService _applicationUserService;
    #endregion

    #region Constructors
    public UserCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer, 
        IMapper mapper, 
        UserManager<User> userManager, 
        IApplicationUserService applicationUserService) : base(stringLocalizer)
    {
        _sharedResources = stringLocalizer;
        _mapper = mapper;
        _userManager = userManager;
        _applicationUserService = applicationUserService;
    }


    #endregion

    #region Handle Functions
    
    #region Create User
    public async Task<GenericBaseResponse<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var identityUser = _mapper.Map<User>(request);
        
        var createResult = await _applicationUserService.AddUserAsync(identityUser, request.Password);

        switch (createResult)
        {
            case "EmailIsExist": return BadRequest<string>(_sharedResources[SharedResourcesKeys.EmailIsExist]);
            case "UserNameIsExist": return BadRequest<string>(_sharedResources[SharedResourcesKeys.UserNameIsExist]);
            case "ErrorInCreateUser": return BadRequest<string>(_sharedResources[SharedResourcesKeys.FaildToAddUser]);
            case "Failed": return BadRequest<string>(_sharedResources[SharedResourcesKeys.TryToRegisterAgain]);
            case "Success": return Success<string>("");
            default: return BadRequest<string>(createResult);
        }
    }
    #endregion

    #region Edit User
    public async Task<GenericBaseResponse<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var oldUser = await _userManager.FindByIdAsync(request.Id.ToString());

        if (oldUser == null) return NotFound<string>();

        var newUser = _mapper.Map(request, oldUser);

        var userByUserName = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == newUser.UserName && x.Id != newUser.Id);

        if (userByUserName != null) return BadRequest<string>(_sharedResources[SharedResourcesKeys.UserNameIsExist]);

        var result = await _userManager.UpdateAsync(newUser);

        if (!result.Succeeded) return BadRequest<string>(_sharedResources[SharedResourcesKeys.UpdateFailed]);

        return Success((string)_sharedResources[SharedResourcesKeys.Updated]);
    }
    #endregion

    #region Delete User
    public async Task<GenericBaseResponse<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
     
        if (user == null) return NotFound<string>();
        
        var result = await _userManager.DeleteAsync(user);
        
        if (!result.Succeeded) return BadRequest<string>(_sharedResources[SharedResourcesKeys.DeletedFailed]);
        return Success((string)_sharedResources[SharedResourcesKeys.Deleted]);
    }
    #endregion

    #region Change Password
    public async Task<GenericBaseResponse<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null) return NotFound<string>();

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded) return BadRequest<string>(result.Errors.FirstOrDefault().Description);
        return Success((string)_sharedResources[SharedResourcesKeys.Success]);
    } 
    #endregion

    #endregion
}
