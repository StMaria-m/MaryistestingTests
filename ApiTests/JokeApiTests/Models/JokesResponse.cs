using System.Collections.Generic;

namespace ApiTests.JokeApiTests.Models
{
    public class JokesResponse
    {
        public bool Error { get; set; }
        public int Amount { get; set; }
        public List<SingleJoke> Jokes { get; set; }
    }
}
