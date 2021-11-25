using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.MyStoreApiTests.Models
{
    public class GetOrderResponse
    {
        public OrderResponse Order { get; set; }
        public SummaryModel Summary { get; set; }
    }
}
