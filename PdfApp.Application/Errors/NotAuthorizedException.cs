using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Errors
{
    public class NotAuthorizedException : CustomError
    {
        private readonly string _errorMessage;

        public NotAuthorizedException(string errorMessage) : base(HttpStatusCode.Unauthorized, errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public override List<KeyValuePair<string, string>> SerializeErrors()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ErrorMessage", _errorMessage)
            };
        }
    }
}
