using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string PersonName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = null!; 
        public string? Address { get; set; }

        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public bool RecieveNewsLetter { get; set; }

        public double? Age { get; set; }


        public Person ToPerson()
        {
            return new Person
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                Address = Address,
                CountryID = CountryID,
                RecieveNewsLetter = RecieveNewsLetter
            }; 
        }


        public override bool Equals(object? obj)
        {
            if(obj is null || obj.GetType() != typeof(PersonResponse))
                return false;

            PersonResponse Person = (PersonResponse)obj;

            return PersonID == Person.PersonID &&
                   PersonName== Person.PersonName&&
                   Email ==Person.Email&&
                   Gender==Person.Gender&&
                   DateOfBirth==Person.DateOfBirth&&
                   Address==Person.Address&&
                   CountryID==Person.CountryID&&
                   RecieveNewsLetter == Person.RecieveNewsLetter;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}"; 
        }


        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                Address = Address,
                CountryID = CountryID,
                RecieveNewsLetter = RecieveNewsLetter,
            };
        }
    }

    public static class PersonExtension
    {

        public static PersonResponse ToPersonResponse(this Person person)
        {

            return new PersonResponse
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                Address = person.Address,
                CountryID = person.CountryID,
                RecieveNewsLetter = person.RecieveNewsLetter,
                Age = (person.DateOfBirth != null) ? (DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25  : null
            }; 
        }
    }
}
