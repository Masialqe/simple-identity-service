namespace IdentityApp.Users.Models
{
    public sealed class Role
    {

        public static Role Create(string name)
            => new Role { Name = name };
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
