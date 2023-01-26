using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfApp.Contracts.Response
{
    public class Response<T>
    {
        public Response()
        {
        }

        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<KeyValuePair<string, string>> Errors { get; set; } = new List<KeyValuePair<string, string>>();
        public int StatusCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
