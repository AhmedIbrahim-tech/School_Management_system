using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Services.Services;

#region Interface
public interface IAuthorizationServiceAsync
{
    public Task<string> AddRoleAsync(string roleName);
    public Task<bool> IsRoleExistByName(string roleName);
    public Task<string> EditRoleAsync(EditRoleRequest request);
    public Task<string> DeleteRoleAsync(int roleId);
    public Task<bool> IsRoleExistById(int roleId);
    public Task<List<Role>> GetRolesList();
    public Task<Role> GetRoleById(int id);
}

#endregion
public class AuthorizationServiceAsync : IAuthorizationServiceAsync
{
    #region Fields
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDBContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Constructors
    public AuthorizationServiceAsync(RoleManager<Role> roleManager,
                                UserManager<User> userManager,
                                ApplicationDBContext dbContext,
                                IUnitOfWork unitOfWork)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }


    #endregion

    #region handle Functions

    #region List of Role
    public async Task<List<Role>> GetRolesList()
    {
        return await _roleManager.Roles.ToListAsync();
    }
    #endregion

    #region GET: Role by Id
    public async Task<Role> GetRoleById(int id)
    {
        return await _roleManager.FindByIdAsync(id.ToString());
    }

    #endregion

    #region Create Role
    public async Task<string> AddRoleAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return "Role name cannot be empty.";
        }

        var identityRole = new Role { Name = roleName };
        var result = await _roleManager.CreateAsync(identityRole);

        return result.Succeeded ? "Success" : "Failed: " + string.Join(", ", result.Errors);
    }

    #endregion

    #region Edit Role

    public async Task<string> EditRoleAsync(EditRoleRequest request)
    {
        // Validate request
        if (request == null || string.IsNullOrWhiteSpace(request.Name))
        {
            return "Invalid request. Role name cannot be empty.";
        }

        // Check if the role exists
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
        {
            return "Role not found.";
        }

        // Update the role
        role.Name = request.Name;
        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            return "Success";
        }

        // Return list of errors if update failed
        return $"Failed to update role: {string.Join(", ", result.Errors)}";
    }

    #endregion

    #region Delete Role
    public async Task<string> DeleteRoleAsync(int roleId)
    {
        // Chech if this role exist or not
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null) return "NotFound";

        // Chech if user has this role or not
        var users = await _userManager.GetUsersInRoleAsync(role.Name);

        // return exception 
        if (users != null && users.Any()) return "Used";

        // delete this role
        var result = await _roleManager.DeleteAsync(role);

        //success
        if (result.Succeeded) return "Success";

        //problem
        return $"Failed to delete role: {string.Join(", ", result.Errors)}";
    }

    #endregion
  
    #region Helper

    #region Check Role is Exists By Name
    public async Task<bool> IsRoleExistByName(string roleName)
    {
        //var role=await _roleManager.FindByNameAsync(roleName);
        //if(role == null) return false;
        //return true;
        return await _roleManager.RoleExistsAsync(roleName);
    }

    #endregion

    #region Check Role is Exists By Id
    public async Task<bool> IsRoleExistById(int roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null) return false;
        else return true;
    }

    #endregion

    #endregion

    #endregion


}
