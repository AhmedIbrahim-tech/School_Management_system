﻿namespace Core.Features.ApplicationUser.Commands.Validations;

public class EditUserValidator : AbstractValidator<EditUserCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    #endregion

    #region Constructors
    public EditUserValidator(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
        ApplyValidationsRules();
    }
    #endregion

    #region Handle Functions
    public void ApplyValidationsRules()
    {
        RuleFor(x => x.FullName)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
             .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.Email)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
    }

    #endregion
}
