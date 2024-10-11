using System.Net.Mail;

namespace ComparativeEstimationLibrary
{
    public class Administration
    {
        private readonly string _fileName = "compest.bat";
        private string _currentUserAsEmail = string.Empty;

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is missing");

            try
            {
                MailAddress m = new(email);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email");
            }
        }

        public string CurrentUserAsEmail
        {
            get
            {
                return _currentUserAsEmail;
            }

            private set
            {
                ValidateEmail(value);
                _currentUserAsEmail = value;
            }
        }

        public Administration(string currentUserAsEmail)
        {
            CurrentUserAsEmail = currentUserAsEmail;
        }

        public Project[] LoadProjectsFromFile()
        {
            throw new NotImplementedException();
        }
    }
}