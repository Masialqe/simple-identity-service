﻿using IdentityApp.Common.Abstractions.ApiResults;
using IdentityApp.Common.Abstractions.Errors;
using IdentityApp.Users.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IdentityApp.Shared.Domain.Errors
{
    public static class UserErrors
    {
        public static Error UserAlreadyExistsError => Error.Conflict("Cannot create a user.");
        public static Error UserBlockedError => Error.Forbidden("Access Denied.");
        public static Error InvalidUserError => Error.Unauthorized("Invalid user credentials.");
        public static Error UserDoesntExistsError => InvalidUserError;
    }
}