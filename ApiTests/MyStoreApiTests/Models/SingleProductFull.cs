using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.MyStoreApiTests.Models
{
    public class SingleProductFull: SingleProduct
    {
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public int Price { get; set; }
        public string Created { get; set; }
        public string Status { get; set; }
        public string Tags { get; set; }
    }
}
