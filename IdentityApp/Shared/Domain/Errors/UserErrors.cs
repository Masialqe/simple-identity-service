using IdentityApp.Shared.Abstractions.Errors;


namespace IdentityApp.Shared.Domain.Errors
{
    public static class UserErrors
    {
        public static Error UserAlreadyExistsError => Error.Conflict("Cannot create a user.");
        public static Error UserBlockedError => Error.Forbidden("Access Denied.");
        public static Error InvalidUserError => Error.Unauthorized("Invalid user credentials.");
        public static Error UserDoesntExistsError => InvalidUserError;

        public static Error UsersPasswordDoesntMatchRequirementsError(string message)
            => Error.BadRequest(message);
    }
}
