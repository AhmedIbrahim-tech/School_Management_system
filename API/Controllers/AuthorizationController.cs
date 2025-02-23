using Core.Features.Authorization.Queries.Requests;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
public class AuthorizationController : GenericBaseController
{

    #region Role List
    [HttpGet(Router.Authorization.RoleList)]
    public async Task<IActionResult> GetRoleList()
    {
        var response = await _mediator.Send(new GetRolesListQuery());
        return ActionResult(response);
    }

    #endregion
    
    #region GET: RoleById
    [HttpGet(Router.Authorization.GetRoleById)]
    public async Task<IActionResult> GetRoleById([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetRoleByIdQuery() { Id = id });
        return ActionResult(response);
    } 
    #endregion

    #region Create Role

    [HttpPost(Router.Authorization.Create)]
    public async Task<IActionResult> Create([FromForm] AddRoleCommand command)
    {
        var response = await _mediator.Send(command);
        return ActionResult(response);
    }


    #endregion

    #region Edit Role

    [HttpPost(Router.Authorization.Edit)]
    public async Task<IActionResult> Edit([FromForm] EditRoleCommand command)
    {
        var response = await _mediator.Send(command);
        return ActionResult(response);
    }


    #endregion

    #region Delete Role
    [HttpDelete(Router.Authorization.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _mediator.Send(new DeleteRoleCommand(id));
        return ActionResult(response);
    }
    #endregion

}
