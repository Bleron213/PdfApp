using FluentValidation;
using Newtonsoft.Json.Linq;
using PdfApp.Contracts.Request;
using WkHtmlToPdfDotNet;

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
                .NotNull()
                .NotEmpty()
                .WithMessage(x => $"{nameof(PdfInput.Options.PageColorMode)} must not be null or empty")
                .Must(value => Enum.TryParse<ColorMode>(value, out _))
                .WithMessage(x => $"Value of property is not assigned to {nameof(ColorMode)} enum");

            RuleFor(x => x.Options.PageOrientation)
                .NotNull()
                .NotEmpty()
                .WithMessage(x => $"{nameof(PdfInput.Options.PageOrientation)} must not be null or empty")
                .Must(value => Enum.TryParse<Orientation>(value, out _))
                .WithMessage(x => $"Value of property is not assigned to {nameof(Orientation)} enum");

            RuleFor(x => x.Options.PagePaperSize)
                .NotNull()
                .NotEmpty()
                .WithMessage(x => $"{nameof(PdfInput.Options.PagePaperSize)} must not be null or empty")
                .Must(value => Enum.TryParse<PaperKind>(value, out _))
                .WithMessage(x => $"Value of property is not assigned to {nameof(PaperKind)} enum");
        }
    }
}
