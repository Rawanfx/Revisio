namespace Revisio.Application.Common.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string ErrorMessage) : base(ErrorMessage) { }
       
    }
}
