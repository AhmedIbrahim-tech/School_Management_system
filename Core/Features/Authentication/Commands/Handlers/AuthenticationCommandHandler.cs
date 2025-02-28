﻿using Microsoft.AspNetCore.Identity;

namespace Core.Features.Authentication.Commands.Handlers;

public class AuthenticationCommandHandler : GenericBaseResponseHandler,
    IRequestHandler<SignInCommand, GenericBaseResponse<JwtAuthResult>>,
    IRequestHandler<RefreshTokenCommand, GenericBaseResponse<JwtAuthResult>>,
    IRequestHandler<SendResetPasswordCommand, GenericBaseResponse<string>>,
    IRequestHandler<ResetPasswordCommand, GenericBaseResponse<string>>

{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAuthenticationServiceAsync _authenticationService;



    #endregion

    #region Constructors
    public AuthenticationCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                        UserManager<User> userManager,
                                        SignInManager<User> signInManager,
                                        IAuthenticationServiceAsync authenticationService) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _signInManager = signInManager;
        _authenticationService = authenticationService;
    }


    #endregion

    #region Handle Functions

    #region Sign in
    public async Task<GenericBaseResponse<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null) return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.UserNameIsNotExist]);

        var signInResult = _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!signInResult.IsCompletedSuccessfully) return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.PasswordNotCorrect]);

        if (!user.EmailConfirmed) return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.EmailNotConfirmed]);

        var result = await _authenticationService.GetJWTToken(user);

        return Success(result);
    }
    #endregion

    #region Refresh Token

    public async Task<GenericBaseResponse<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = _authenticationService.ReadJWTToken(request.AccessToken);
        var ResultValidateAndCheckJWT = await _authenticationService.ValidateAndCheckJWTDetails(jwtToken, request.AccessToken, request.RefreshToken);
        switch (ResultValidateAndCheckJWT)
        {
            case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.AlgorithmIsWrong]);
            case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.TokenIsNotExpired]);
            case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.RefreshTokenIsNotFound]);
            case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.RefreshTokenIsExpired]);
        }
        var (userId, expiryDate) = ResultValidateAndCheckJWT;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound<JwtAuthResult>();

        var result = await _authenticationService.GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);
        return Success(result);
    }

    #endregion

    #region Reset Password
    public async Task<GenericBaseResponse<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.SendResetPasswordCode(request.Email);
        switch (result)
        {
            case "UserNotFound": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            case "ErrorInUpdateUser": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryAgainInAnotherTime]);
            case "Failed": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryAgainInAnotherTime]);
            case "Success": return Success<string>("");
            default: return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryAgainInAnotherTime]);
        }
    }

    public async Task<GenericBaseResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.ResetPassword(request.Email, request.Password);
        switch (result)
        {
            case "UserNotFound": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            case "Failed": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvaildCode]);
            case "Success": return Success<string>("");
            default: return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvaildCode]);
        }
    } 
    #endregion

    #endregion
}
