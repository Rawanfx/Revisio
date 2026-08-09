
namespace Revisio.Application.Common.Interfaces;

public interface ITextExtractor
{
    string Extract(Stream pdfStream);
    bool CanExtract(string extension);
}
