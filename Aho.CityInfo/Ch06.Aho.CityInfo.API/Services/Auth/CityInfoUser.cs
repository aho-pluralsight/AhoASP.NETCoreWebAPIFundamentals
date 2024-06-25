namespace Ch06.Aho.CityInfo.API.Services.Auth
{
    public class CityInfoUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }

        public CityInfoUser(string userId, string userName, string firstName, string lastName, string role)
        {
            UserId = userId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }
    }
}
