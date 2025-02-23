namespace Core.Mapping.ApplicationUser;

public class ApplicationUserProfile : Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<AddUserCommand, User>();

        CreateMap<EditUserCommand, User>();

        CreateMap<User, GetUserPaginationReponse>();

        CreateMap<User, GetUserByIdResponse>();
    }

}
