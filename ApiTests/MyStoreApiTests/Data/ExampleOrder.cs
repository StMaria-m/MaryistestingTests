using ApiTests.MyStoreApiTests.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.MyStoreApiTests.Data
{
    public class ExampleOrder
    {
        public static object[] Orders =
        {
           new object[]
           {
               new NewOrderRequest
               {
               Customer = "Wanda Nowak",
               Address = "Warszawa, ul. Nowa 55"
               }
           },

            new object[]
            {
               new NewOrderRequest
               {
               Customer = "Jerzy Zielilński",
               Address = "Warszawa, ul. Szeroka  155"
               }
            }
        };
    }
}
