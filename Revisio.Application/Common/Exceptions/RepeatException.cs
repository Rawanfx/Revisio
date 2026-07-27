namespace Revisio.Application.Common.Exceptions
{
    public class RepeatException:Exception
    {
        public RepeatException(string errorMessage) : base(errorMessage) { }
    }
}
