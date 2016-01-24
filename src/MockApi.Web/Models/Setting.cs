using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using LiteDB;

namespace MockApi.Web.Models
{
    public class Setting
    {
        public ObjectId Id { get; set; }

        [Required]
        [StartsWith('/')]
        public string Route { get; set; }

        [Required]
        [Display(Name = "Status Code")]
        [DefaultValue(HttpStatusCode.OK)]
        public HttpStatusCode StatusCode { get; set; }

        [Required]
        [ValidJson]
        [Display(Name = "Response JSON")]
        public string ResponseJson { get; set; }

        [Required]
        [DefaultValue(HttpMethodType.Get)]
        [Display(Name = "HTTP Method")]
        public HttpMethodType HttpMethod { get; set; }

        public Setting()
        {
            StatusCode = HttpStatusCode.OK;
            HttpMethod = HttpMethodType.Get;
        }
    }
}