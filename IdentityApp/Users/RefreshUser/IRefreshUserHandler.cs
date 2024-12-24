namespace IdentityApp.Users.RefreshUser
{
    public interface IRefreshUserHandler
    {
        Task<IResult> Handle(RefreshUserRequest request);
    }
}