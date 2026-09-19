using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Person
    {
        public Guid PersonID { get; set; }
        [Required(ErrorMessage = "Name can't empty or null")]
        public string PersonName { get; set; } = null!; 

        [Required(ErrorMessage = "Email can't empty or null")]
        [EmailAddress(ErrorMessage = "Invalid Email ")]
        public string Email { get; set; } = null!; 
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = null!; 
        public string? Address { get; set; }

        public Guid? CountryID { get; set; }
        public bool RecieveNewsLetter { get; set; }
    }
}
