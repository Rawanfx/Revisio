namespace Revisio.Application.Common.Interfaces
{
    public interface ITextExtractorFactory
    {
        ITextExtractor textExtractor(string fileExtension);
    }
}
