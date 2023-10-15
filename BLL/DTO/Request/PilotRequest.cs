using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Request;

public class PilotRequest
{
    public int Id { get; set; }
    
    [MaxLength(30, ErrorMessage = "FirstName Max length is 30.")]
    [Required(ErrorMessage = "FirstName is required.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "LastName is required.")]
    [MaxLength(30, ErrorMessage = "LastName Max length is 30.")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Age is required.")]
    [Range(21, 60, ErrorMessage = "Age must be in range 21, 60")]
    public int Age { get; set; }
    
    [Range(0, 10, ErrorMessage = "Rating must be in range 0.0, 10.0")]
    [Required(ErrorMessage = "Rating is required.")]
    public double Rating { get; set; }
}
