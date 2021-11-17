using ApiTests.LoveCalculatorApiTests.Models;

namespace ApiTests.LoveCalculatorApiTests.Data
{
    public class NameSamples
    {
        public static object[] Names =
        {
            new object[]
            {
                "John", "Aneta", "56"
            },
            new object[]
            {
                "John", "Aneto", "46"
            },
            new object[]
            {
                "John", "Anetk", "36"
            }
        };

        public static object[] SingleLoveData =
        {
            new object[]
            {
                new SingleLoveResponse 
                {
                    Fname = "John",
                    Sname = "Aneta",
                    Percentage = "56",
                }
            },
            new object[]
            {
                new SingleLoveResponse
                {
                    Fname = "John",
                    Sname = "Aneto",
                    Percentage = "46",
                }
            },
            new object[]
            {
                new SingleLoveResponse
                {
                    Fname = "John",
                    Sname = "Anetk",
                    Percentage = "36",
                }
            }
        };
    }
}
