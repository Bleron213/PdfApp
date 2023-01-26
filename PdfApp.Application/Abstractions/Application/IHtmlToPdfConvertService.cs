using PdfApp.Contracts.Request;

namespace PdfApp.Application.Abstractions.Application
{
    public interface IHtmlToPdfConvertService
    {
        byte[] ConvertToPdf(PdfInput input);
    }
}
