namespace Core.Features.Students.Commands.Handlers;

public class StudentCommandHandler : GenericBaseResponseHandler,
    IRequestHandler<AddStudentCommand, GenericBaseResponse<int>>,
    IRequestHandler<EditStudentCommand, GenericBaseResponse<int>>,
    IRequestHandler<DeleteStudentCommand, GenericBaseResponse<int>>

{

    #region Fields
    private readonly IStudentServices _studentServices;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    #endregion

    #region Contractor (s)
    public StudentCommandHandler(IStudentServices studentServices, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
    {
        _studentServices = studentServices;
        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }
    #endregion

    #region Handler Function

    #region Handle of Create
    public async Task<GenericBaseResponse<int>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
    {
        var Mapper = _mapper.Map<Student>(request);

        var response = await _studentServices.AddAsync(Mapper);

        if (response > 0) return Created<int>(response);
        else return NotFound<int>();
    }
    #endregion

    #region Handle of Edit
    public async Task<GenericBaseResponse<int>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
    {
        var CurrentStudent = await _studentServices.GetStudentsByIdAsync(request.Id);

        if (CurrentStudent == null) return NotFound<int>();

        var Mapper = _mapper.Map(request, CurrentStudent);

        var response = await _studentServices.EditAsync(Mapper);

        if (response == 1) return Updated<int>(response);
        else return NotFound<int>();

    }
    #endregion

    #region Handle of Delete
    public async Task<GenericBaseResponse<int>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var CurrentStudent = await _studentServices.GetStudentsByIdAsync(request.Id);

        if (CurrentStudent == null) return NotFound<int>();

        var response = await _studentServices.DeleteAsync(CurrentStudent);

        if (response == 1) return Delete<int>();
        else return NotFound<int>();


    }
    #endregion

    #endregion
}
