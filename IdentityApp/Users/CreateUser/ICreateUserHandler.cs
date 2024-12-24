
namespace IdentityApp.Users.CreateUser
{
    public interface ICreateUserHandler
    {
        Task<IResult> Handle(CreateUserRequest request);
    }
}