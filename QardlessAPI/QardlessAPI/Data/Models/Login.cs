namespace QardlessAPI.Data.Models
{
    public class Login
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool isLoggedIn { get; set; }
    }
}
