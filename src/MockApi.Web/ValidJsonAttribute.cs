using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace MockApi.Web
{
    public class ValidJsonAttribute : ValidationAttribute
    {
        public ValidJsonAttribute()
        {
            ErrorMessage = "Input was not a valid JSON object format";
        }

        public override bool IsValid(object value)
        {
            var input = value?.ToString().Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            if (!(input.StartsWith("{") && input.EndsWith("}")))
            {
                return false;
            }

            try
            {
                JObject.Parse(input);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}