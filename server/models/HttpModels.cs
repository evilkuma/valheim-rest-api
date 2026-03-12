using System;
using System.Net;
using System.Collections.Generic;

namespace ValheimRestApi.Models
{
    public class ResponseEventArgs : EventArgs
    {
        public HttpListenerResponse Response { get; }

        public ResponseEventArgs(HttpListenerResponse response)
        {
            Response = response;
        }
    }
}
