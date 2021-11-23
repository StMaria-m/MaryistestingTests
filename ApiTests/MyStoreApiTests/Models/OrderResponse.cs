using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.MyStoreApiTests.Models
{
    public class OrderResponse
    {
        
        public int Id { get; set; }
        public string Created { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
    }

}
