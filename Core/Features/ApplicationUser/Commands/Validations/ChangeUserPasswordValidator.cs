﻿namespace Core.Features.ApplicationUser.Commands.Validations;

public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    #endregion

    #region Constructors
    public ChangeUserPasswordValidator(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
        ApplyValidationsRules();
    }
    #endregion

    #region Handle Functions
    public void ApplyValidationsRules()
    {

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        RuleFor(x => x.CurrentPassword)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        RuleFor(x => x.NewPassword)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        RuleFor(x => x.ConfirmPassword)
             .Equal(x => x.NewPassword).WithMessage(_localizer[SharedResourcesKeys.PasswordNotEqualConfirmPass]);

    }

    #endregion
}
