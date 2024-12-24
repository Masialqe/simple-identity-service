using IdentityApp.Endpoints.Validation;
using IdentityApp.Endpoints;
using FluentValidation;


namespace IdentityApp.Users.LoginUser
{
    public sealed class Validator : AbstractValidator<LoginUserRequest>
    {
        public Validator()
        {
            RuleFor(r => r.login).NotEmpty().MinimumLength(5);
            RuleFor(r => r.password).NotEmpty().MinimumLength(5);
        }
    }

    public sealed class LoginUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/login", async (LoginUserRequest request, ILoginUserHandler handler) =>
            {
                return await handler.Handle(request);
            })
               .AddEndpointFilter<ValidationActionFilter<LoginUserRequest>>()
               .WithTags("Authentication");
        }
    }
}
