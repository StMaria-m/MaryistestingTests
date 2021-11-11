using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.MyStoreApiTests.Models
{
    public class ProductsGridResponse
    {
        public List<SingleProduct> Products { get; set; }
        public Summary Summary { get; set; }
    }

}

