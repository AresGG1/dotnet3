namespace BLL.DTO.Response;

public class PilotResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public double Rating { get; set; }
    public List<AircraftResponse> Aircrafts;
}