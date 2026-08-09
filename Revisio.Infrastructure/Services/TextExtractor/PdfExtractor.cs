using Revisio.Application.Common.Interfaces;
using System.Text;
using UglyToad.PdfPig;

namespace Revisio.Infrastructure.Services
{
    public class PdfExtractor : ITextExtractor
    {
        public bool CanExtract(string extension) => extension == ".pdf";
        
        public string Extract(Stream pdfStream)
        {
            var textBuilder = new StringBuilder();
            using (var document = PdfDocument.Open(pdfStream))
            {
                foreach (var page in document.GetPages())
                {
                    textBuilder.AppendLine(page.Text);
                }
            }
            return textBuilder.ToString();
        }
    }
}
