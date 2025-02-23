namespace Core.Features.Authentication.Queries.Requests;

public class AuthorizeUserQuery : IRequest<GenericBaseResponse<string>>
{
    public string AccessToken { get; set; }
}
