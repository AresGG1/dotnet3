namespace BLL.Exceptions;

public class AlreadyAssignedException : Exception
{
    public AlreadyAssignedException(string message)
        : base(message)
    {
    }

    public AlreadyAssignedException()
        : base()
    {
    }
}