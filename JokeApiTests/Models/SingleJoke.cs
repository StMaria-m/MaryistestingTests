namespace JokeApiTests.Models
{
    public class SingleJoke
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Joke { get; set; }
        public bool Safe { get; set; }
        public string Lang { get; set; }
        public Flags Flags { get; set; }
    }
}
