namespace QardlessAPI.Data.Models
{
    public class Login
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool isLoggedin
        {
            get { return isLoggedin; }

            set { isLoggedin = false; }
        }
    }
}
