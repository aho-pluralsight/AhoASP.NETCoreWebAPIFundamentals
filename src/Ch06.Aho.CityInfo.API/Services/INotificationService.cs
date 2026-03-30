namespace Ch06.Aho.CityInfo.API.Services
{
    public interface INotificationService
    {
        void Notify(string subject, string note);
    }
}