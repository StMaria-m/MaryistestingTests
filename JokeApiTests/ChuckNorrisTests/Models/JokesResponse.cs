using System.Collections.Generic;

namespace ApiTests.ChuckNorrisTests.Models
{
    public class JokesResponse
    {
        public int Total { get; set; }
        public List<SingleJokeResponse> Result { get; set; }
    }
}
