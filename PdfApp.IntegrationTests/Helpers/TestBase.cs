using PdfApp.IntegrationTests.Helpers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.IntegrationTests.Helpers
{
    public class TestBase : IDisposable
    {
        private PdfAppRestFactory _pdfAppRestFactory;
        private readonly Func<PdfAppRestFactory> _pdfAppRestFactoryFactory;

        public TestBase()
        {
            _pdfAppRestFactoryFactory = () => new PdfAppRestFactory();
        }
        public PdfAppRestFactory pdfAppRestFactory => _pdfAppRestFactory ??= _pdfAppRestFactoryFactory();

        public void Dispose()
        {
            // Dispose un
        }
    }
}
