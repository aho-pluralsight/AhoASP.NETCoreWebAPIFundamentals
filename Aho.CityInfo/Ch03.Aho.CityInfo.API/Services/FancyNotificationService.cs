namespace Ch03.Aho.CityInfo.API.Services
{
    public class FancyNotificationService : INotificationService
    {
        private string _noteTo = "admin@aho.dz";
        private string _noteFrom = "fancy.info@aho.dz";

        public void Notify(string subject, string note)
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Note from {_noteFrom} to {_noteTo}, with {this.GetType().Name}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Note: {note}");
            Console.ResetColor();
        }
    }
}
