namespace IdentityApp.Users.CreateRole
{
    public interface ICreateRoleHandler
    {
        Task<IResult> Handle(CreateRoleRequest request);
    }
}
