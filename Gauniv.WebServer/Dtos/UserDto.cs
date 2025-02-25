using Gauniv.WebServer.Data;

namespace Gauniv.WebServer.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public List<Game> PurchasedGames { get; set; } = new List<Game>();
    }
}
