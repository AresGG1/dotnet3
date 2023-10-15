namespace BLL.Exceptions;

public class NotAssignedException : Exception
{
    public NotAssignedException(string message)
        : base(message)
    {
    }

    public NotAssignedException()
        : base()
    {
    }
}