namespace DAL.Parameters;

public class PilotParameters : QueryStringParameters
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? Age { get; set; }
    public double? Rating { get; set; }
}   
