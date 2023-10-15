namespace BLL.Exceptions;

public class ExperienceException : Exception
{
    public ExperienceException(string message)
        : base(message)
    {
    }

    public ExperienceException()
        : base()
    {
    }
}