using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using LiteDB;

namespace MockApi.Web.Models
{
    public class Mock
    {
        public ObjectId Id { get; set; }

        [StartsWith('/')]
        [Required]
        public string Path { get; set; }

        [Required]
        [DefaultValue(HttpMethodType.Get)]
        public HttpMethodType Verb { get; set; }

        public List<MockResponse> Responses { get; set; }

        [Required]
        [Display(Name = "Active Status Code")]
        [DefaultValue(HttpStatusCode.OK)]
        public HttpStatusCode ActiveStatusCode { get; set; }

        public Mock()
        {
            ActiveStatusCode = HttpStatusCode.OK;
            Responses = new List<MockResponse>();
        }
    }
}