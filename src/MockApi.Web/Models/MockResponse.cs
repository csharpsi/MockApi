using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MockApi.Web.Models
{
    public class MockResponse
    {
        [Required]
        [Display(Name = "Status Code")]
        public HttpStatusCode StatusCode { get; set; }

        [Required]
        [Display(Name = "Response Data")]
        [ValidJson]
        public string Data { get; set; }

        public MockResponse()
        {
            
        }

        public MockResponse(HttpStatusCode statusCode, string data)
        {
            StatusCode = statusCode;
            Data = data;
        }
    }
}