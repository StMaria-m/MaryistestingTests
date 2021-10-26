using System.Collections.Generic;

namespace ApiTests.ReqresTests.Models
{
    public class UnknownsListResponse
    {
        public int Page { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public int Total_pages { get; set; }
        public List<SingleUnknown> Data { get; set; }
        public SupportContains Support { get; set; }
    }
}
