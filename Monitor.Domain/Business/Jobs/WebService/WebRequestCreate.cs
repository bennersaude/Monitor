using System;
using System.Net;

namespace Monitor.Domain.Business.Jobs.WebService
{
    public class WebRequestCreate : IWebRequestCreate
    {
        public WebRequest Create(Uri uri)
        {
            return WebRequest.Create(uri);
        }
    }
}