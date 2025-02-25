namespace Gauniv.WebServer.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Relation inverse Many-to-Many avec les jeux
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
