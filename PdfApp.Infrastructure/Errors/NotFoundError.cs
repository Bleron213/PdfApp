using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Infrastructure.Errors
{
    public class NotFoundError : CustomError
    {
        private readonly string _message;

        public NotFoundError(string message) : base(HttpStatusCode.NotFound, message)
        {
            _message = message;
        }

        public override List<KeyValuePair<string, string>> SerializeErrors()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ErrorMessage", _message)
            };
        }
    }
}
