using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }
        [Required(ErrorMessage = "Name can't empty or null")]
        [StringLength(40)]
        public string PersonName { get; set; } = null!; 


        [Required(ErrorMessage = "Email can't empty or null")]
        [EmailAddress(ErrorMessage = "Invalid Email ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!; 
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = null!;
        [StringLength(200)]
        public string? Address { get; set; }

        public Guid? CountryID { get; set; }

        public bool RecieveNewsLetter { get; set; }

        [ForeignKey("CountryID")]
        public Country? Country { get; set; }
    }
}
