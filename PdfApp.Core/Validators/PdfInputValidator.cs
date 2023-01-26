using FluentValidation;
using PdfApp.Contracts.Enums;
using PdfApp.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Core.Validators
{
    public class PdfInputValidator : AbstractValidator<PdfInput>
    {
        public PdfInputValidator()
        {
            RuleFor(x => x.HtmlString)
                .NotEmpty()
                .NotNull()
                .WithMessage(x => $"{nameof(PdfInput.HtmlString)} must not be null or empty");

            RuleFor(x => x.Options)
                .NotNull()
                .WithMessage(x => $"{nameof(PdfInput.Options)} must not be null");

            RuleFor(x => x.Options.PageColorMode)
                .IsInEnum()
                .WithMessage(x => $"{nameof(PdfInput.Options.PageColorMode)} must be defined in Enum");

            RuleFor(x => x.Options.PageOrientation)
                .IsInEnum()
                .WithMessage(x => $"{nameof(PdfInput.Options.PageOrientation)} must be defined in Enum");

            RuleFor(x => x.Options.PagePaperSize)
                .IsInEnum()
                .WithMessage(x => $"{nameof(PdfInput.Options.PagePaperSize)} must be defined in Enum");
        }
    }
}
