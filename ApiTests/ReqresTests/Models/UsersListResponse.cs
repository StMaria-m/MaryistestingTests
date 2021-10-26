using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTests.ReqresTests.Models
{
    public class UsersListResponse
    {
        public int Page { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public int Total_pages { get; set; }
        public List<SingleUser> Data { get; set; }
        public SupportContains Support { get; set; }
    }
}
