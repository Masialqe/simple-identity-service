namespace IdentityApp.Users.LoginUser
{
    public interface ILoginUserHandler
    {
        Task<IResult> Handle(LoginUserRequest request);
    }
}