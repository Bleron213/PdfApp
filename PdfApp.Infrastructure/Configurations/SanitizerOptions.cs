namespace PdfApp.Infrastructure.Configurations
{
    public class SanitizerOptions
    {
        public List<string> AllowedHtmlTags { get; set; }
        public List<string> AllowedHtmlAttributes { get; set; }
    }
}
