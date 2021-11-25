using ApiTests.MyStoreApiTests.Models;

namespace ApiTests.MyStoreApiTests.Data
{
    public class ExampleProduct
    {
        public static object[] Products =
        {
           new object[]
           {
               new NewProductRequest
               {
                Name = "chair",
                Price = 33,
                Manufacturer = "abc",
                Category = "furniture",
                Description = "black",
                Tags = "chairs"
               }
           },
           new object[]
           {
               new NewProductRequest
               {
                Name = "bed",
                Price = 330,
                Manufacturer = "abcabc",
                Category = "furniture",
                Description = "big",
                Tags = "beds, furniture"
               }
           }
        };
    }
}
