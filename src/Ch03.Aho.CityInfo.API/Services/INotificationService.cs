namespace Ch03.Aho.CityInfo.API.Services
{
    public interface INotificationService
    {
        void Notify(string subject, string note);
    }
}