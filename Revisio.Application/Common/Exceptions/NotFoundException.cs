namespace Revisio.Application.Common.Exceptions
{
    public class NotFoundException:Exception
    {
        public string ErrorMessage { get; }
        public NotFoundException(string error)
        {
            this.ErrorMessage = error;
        }
    }
}
