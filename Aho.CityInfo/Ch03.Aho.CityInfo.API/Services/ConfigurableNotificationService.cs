namespace Ch03.Aho.CityInfo.API.Services
{
    public class ConfigurableNotificationService : INotificationService
    {
        private string _noteTo = string.Empty;
        private string _noteFrom = string.Empty;
        private readonly IConfiguration _configuration;

        public ConfigurableNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitParams();
        }

        private void InitParams()
        {
            _noteTo = _configuration["notificationSetings:noteTo"];
            _noteFrom = _configuration["notificationSetings:noteFrom"];
        }

        public void Notify(string subject, string note)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Note from {_noteFrom} to {_noteTo}, with {this.GetType().Name}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Note: {note}");
            Console.ResetColor();
        }
    }
}
