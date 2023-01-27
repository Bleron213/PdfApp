using FluentValidation;
using PdfApp.Contracts.Enums;
using PdfApp.Contracts.Request;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Validators
{
    public class PdfInputValidator : AbstractValidator<PdfInput>
    {
        public PdfInputValidator()
        {
            RuleFor(request => request)
                .NotNull()
                .WithMessage($"{nameof(PdfInput)} cannot be null");

            RuleFor(x => x.HtmlString)
                .NotEmpty()
                .NotNull()
                .WithMessage(x => $"{nameof(PdfInput.HtmlString)} must not be null or empty")
                .Custom((htmlBase64String, ctx) =>
                {
                    Span<byte> buffer = new Span<byte>(new byte[htmlBase64String.Length]);
                    bool isBase64 = Convert.TryFromBase64String(htmlBase64String, buffer, out _);

                    if (!isBase64)
                    {
                        ctx.AddFailure("Html String input is not valid base64 string.");
                        return;
                    }
                });

            RuleFor(x => x.Options)
                .NotNull()
                .WithMessage(x => $"{nameof(PdfInput.Options)} must not be null");

            RuleFor(x => x.Options.PageColorMode)
                .IsInEnum()
                .WithMessage(x => $"{nameof(PdfInput.Options.PageColorMode)} must be defined in Enum")
                .NotEqual(PageColorMode.NotDefined)
                .WithMessage(x => $"{nameof(PdfInput.Options.PageColorMode)} must not be undefined");

            RuleFor(x => x.Options.PageOrientation)
                .IsInEnum()
                .WithMessage(x => $"{nameof(PdfInput.Options.PageOrientation)} must be defined in Enum")
                .NotEqual(PageOrientation.NotDefined)
                .WithMessage(x => $"{nameof(PdfInput.Options.PageOrientation)} must not be undefined");

            RuleFor(x => x.Options.PagePaperSize)
                .IsInEnum()
                .WithMessage(x => $"{nameof(PdfInput.Options.PagePaperSize)} must be defined in Enum")
                .NotEqual(PagePaperSize.NotDefined)
                .WithMessage(x => $"{nameof(PdfInput.Options.PagePaperSize)} must not be undefined");
        }
    }
}
