namespace IdentityApp.Users.Models
{
    public class User
    {
        public static User Create(string login, string passwordHash)
            => new User { Login = login, PasswordHash = passwordHash };

        public void AddRole(Role role)
        {
            if (!Roles.Any(x => x.Name == role.Name)) Roles.Add(role);
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int LoginAttemps { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public string SourceAddres { get; set; } = string.Empty;
        public DateTime BlockExpireOnUtc {  get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
