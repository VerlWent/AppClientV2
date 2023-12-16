namespace ApplicationClientMVC.ViewModels
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string? PasswordHash { get; set; }
    }
}
