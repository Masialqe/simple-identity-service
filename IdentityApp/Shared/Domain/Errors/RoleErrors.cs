using IdentityApp.Common.Abstractions.Errors;

namespace IdentityApp.Shared.Domain.Errors
{
    public static class RoleErrors
    {
        public static Error RoleAlreadyExists => Error.Conflict("Role of given name already exists.");
    }
}
