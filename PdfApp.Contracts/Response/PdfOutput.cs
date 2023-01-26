namespace PdfApp.Contracts.Response
{
    public class PdfOutput
    {
        public string? PdfDocument { get; private set; }
        public int? PdfDocumentSize { get; private set; }

        public PdfOutput(string pdfDocument, int pdfDocumentSize)
        {
            PdfDocument = pdfDocument;
            PdfDocumentSize = pdfDocumentSize;
        }
    }
}
