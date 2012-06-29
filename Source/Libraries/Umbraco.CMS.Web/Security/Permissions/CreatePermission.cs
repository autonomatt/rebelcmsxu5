using Umbraco.Framework.Persistence.Model.Constants;
using Umbraco.Framework.Security;

namespace Umbraco.Cms.Web.Security.Permissions
{
    [Permission(FixedPermissionIds.Create, "Create", FixedPermissionTypes.EntityAction, UserType.User)]
    public class CreatePermission : Permission
    { }
}