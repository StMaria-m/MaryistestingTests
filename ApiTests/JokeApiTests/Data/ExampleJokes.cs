using ApiTests.JokeApiTests.Models;

namespace ApiTests.JokeApiTests.Data
{
    public class ExampleJokes
    {
        public static object[] Jokes =
        {
           new object[]
           {
                new NewJokeRequest
                {
                    Category = "Dark",
                    FormatVersion = 3,
                    Joke = "Example dark joke",
                    Lang = "en",
                    Type = "single",
                     Flags = new Flags
                     {
                        Explicit = false,
                        Nsfw = false,
                        Political = true,
                        Racist = false,
                        Religious = false,
                        Sexist = false
                     }
                }
           },   
            new object[]
            {
                new NewJokeRequest
                {
                    Category = "Programming",
                    FormatVersion = 3,
                    Joke = "Example programming joke",
                    Lang = "de",
                    Type = "two-part",
                     Flags = new Flags
                    {
                        Explicit = false,
                        Nsfw = false,
                        Political = false,
                        Racist = false,
                        Religious = false,
                        Sexist = false
                    }
                }
            }
        };
    }
}
