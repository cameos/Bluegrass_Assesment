using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Blue_API.Custom
{
    public class CustomJsonFormatter:JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            headers.ContentType = new MediaTypeHeaderValue("application/json");
            base.SetDefaultContentHeaders(type, headers, mediaType);
        }
    }
}