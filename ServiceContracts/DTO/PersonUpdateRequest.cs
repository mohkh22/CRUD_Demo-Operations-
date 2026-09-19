using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Person ID can't empty or null")]

        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Name can't empty or null")]
        public string PersonName { get; set; } = null!;

        [Required(ErrorMessage = "Email can't empty or null")]
        [EmailAddress(ErrorMessage = "Invalid Email ")]
        public string Email { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public GenderOptions Gender { get; set; }
        public string? Address { get; set; }

        public Guid? CountryID { get; set; }
        public bool RecieveNewsLetter { get; set; }


        public Person ToPerson()
        {
            return new Person
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                Address = Address,
                CountryID = CountryID,
                RecieveNewsLetter = RecieveNewsLetter

            };
        }
    }
}
