namespace BLL.Exceptions;

public class ToManyAircraftsException : Exception
{
    public ToManyAircraftsException(string message)
        : base(message)
    {
    }

    public ToManyAircraftsException()
        : base()
    {
    }
}