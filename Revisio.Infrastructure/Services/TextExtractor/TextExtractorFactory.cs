
using Revisio.Application.Common.Interfaces;

namespace Revisio.Infrastructure.Services.TextExtractor
{
    public class TextExtractorFactory : ITextExtractorFactory
    {
        private readonly IEnumerable<ITextExtractor> extractors;
        public TextExtractorFactory(IEnumerable<ITextExtractor> extractors)
        {
            this.extractors = extractors;
        }
        public ITextExtractor textExtractor(string fileExtension)
        {
            var validExtractor = extractors.FirstOrDefault(x => x.CanExtract(fileExtension));
            if (validExtractor != null)
                return validExtractor;
            throw new NotSupportedException("Not Supported file")
        }
    }
}
