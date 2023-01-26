using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Errors
{
    public class RequestValidationError : CustomError
    {
        private readonly List<ValidationFailure> _validationFailures;

        public RequestValidationError(List<ValidationFailure> validationFailures) : base("Invalid Request Parameters")
        {
            _validationFailures = validationFailures;
        }

        public override List<KeyValuePair<string, string>> SerializeErrors()
        {
            return _validationFailures.Select(x => new KeyValuePair<string, string>(x.PropertyName, x.ErrorMessage)).ToList();
        }
    }
}
