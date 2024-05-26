namespace Ch03.Aho.CityInfo.API.Services
{
    public class SimpleNotificationService
    {
        private string _noteTo = "admin@aho.dz";
        private string _noteFrom = "info@aho.dz";

        public void Notify(string subject, string note)
        {
            Console.WriteLine($"Note from {_noteFrom} to {_noteTo}, with {this.GetType().Name}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Note: {note}");
        }
    }
}
