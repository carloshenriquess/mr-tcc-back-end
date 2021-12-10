namespace doe.rapido.api.Models
{
    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }

        public class ConfirmEmail
        {
            public string email { get; set; }
            public int? code { get; set; }
        }
        public class ChangePassword
        {
            public string password { get; set; }
            public string email { get; set; }
            public int? code { get; set; }
        }
        public class ChangeLogin
        {
            public string email { get; set; }
            public int? code { get; set; }
        }
        public class EmailToChangePassword
        {
            public string email { get; set; }
        }
        
    }
}
