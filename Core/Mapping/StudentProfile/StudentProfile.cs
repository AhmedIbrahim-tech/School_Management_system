namespace Core.Mapping.StudentProfile;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        MapStudentList();

        MapSingleStudent();

        MapAddStudent();

        MapEditStudent();

        MapDeleteStudent();
    }

    private void MapStudentList()
    {
        CreateMap<Student, GetStudentListResponse>()
            .ForMember(destination => destination.DepartmentName, options => 
                options.MapFrom(sour => sour.Department != null ? sour.Department.DNameEn : string.Empty))
            .ForMember(destination => destination.Name, options => 
                options.MapFrom(sour => GeneralLocalizeEntity.GeneralLocalize(sour.NameAr, sour.NameEn)))
            .ReverseMap();
    }
    
    private void MapSingleStudent()
    {
        CreateMap<Student, GetSingleStudentResponse>()
            .ForMember(destination => destination.DepartmentName, options => 
                options.MapFrom(sour => sour.Department != null ? sour.Department.DNameEn : string.Empty))
            .ForMember(destination => destination.Name, options => 
                options.MapFrom(sour => GeneralLocalizeEntity.GeneralLocalize(sour.NameAr, sour.NameEn)))
            .ReverseMap();
    }
    
    private void MapAddStudent()
    {
        CreateMap<AddStudentCommand, Student>()
            .ForMember(destination => destination.DID, options => 
                options.MapFrom(sour => sour.DepartmentId))
            .ForMember(destination => destination.NameAr, options => 
                options.MapFrom(sour => sour.NameAr))
            .ForMember(destination => destination.NameEn, options => 
                options.MapFrom(sour => sour.NameEn))
            .ReverseMap();
    }
    
    
    private void MapEditStudent()
    {
        CreateMap<EditStudentCommand, Student>()
            .ForMember(destination => destination.DID, options => 
                options.MapFrom(sour => sour.DepartmentId))
            .ForMember(destination => destination.StudID, options => 
                options.MapFrom(sour => sour.Id));
    }

    
    private void MapDeleteStudent()
    {
        CreateMap<DeleteStudentCommand, Student>();
    }
}
