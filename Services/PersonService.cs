using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private List<Person> persons;
        private readonly ICountryService countriesService;

        public PersonService(bool initialize = true)
        {
            persons = new List<Person>();
            countriesService = new CountryService();

            if (initialize)
            {
                persons.Add(new Person() { PersonID = Guid.Parse("8082ED0C-396D-4162-AD1D-29A13F929824"), PersonName = "Aguste", Email = "aleddy0@booking.com", DateOfBirth = DateTime.Parse("1993-01-02"), Gender = "Male", Address = "0858 Novick Terrace", RecieveNewsLetter = false, CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B") });

                persons.Add(new Person() { PersonID = Guid.Parse("06D15BAD-52F4-498E-B478-ACAD847ABFAA"), PersonName = "Jasmina", Email = "jsyddie1@miibeian.gov.cn", DateOfBirth = DateTime.Parse("1991-06-24"), Gender = "Female", Address = "0742 Fieldstone Lane", RecieveNewsLetter = true, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                persons.Add(new Person() { PersonID = Guid.Parse("D3EA677A-0F5B-41EA-8FEF-EA2FC41900FD"), PersonName = "Kendall", Email = "khaquard2@arstechnica.com", DateOfBirth = DateTime.Parse("1993-08-13"), Gender = "Male", Address = "7050 Pawling Alley", RecieveNewsLetter = false, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                persons.Add(new Person() { PersonID = Guid.Parse("89452EDB-BF8C-4283-9BA4-8259FD4A7A76"), PersonName = "Kilian", Email = "kaizikowitz3@joomla.org", DateOfBirth = DateTime.Parse("1991-06-17"), Gender = "Male", Address = "233 Buhler Junction", RecieveNewsLetter = true, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                persons.Add(new Person() { PersonID = Guid.Parse("F5BD5979-1DC1-432C-B1F1-DB5BCCB0E56D"), PersonName = "Dulcinea", Email = "dbus4@pbs.org", DateOfBirth = DateTime.Parse("1996-09-02"), Gender = "Female", Address = "56 Sundown Point", RecieveNewsLetter = false, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                persons.Add(new Person() { PersonID = Guid.Parse("A795E22D-FAED-42F0-B134-F3B89B8683E5"), PersonName = "Corabelle", Email = "cadams5@t-online.de", DateOfBirth = DateTime.Parse("1993-10-23"), Gender = "Female", Address = "4489 Hazelcrest Place", RecieveNewsLetter = false, CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D") });

                persons.Add(new Person() { PersonID = Guid.Parse("3C12D8E8-3C1C-4F57-B6A4-C8CAAC893D7A"), PersonName = "Faydra", Email = "fbischof6@boston.com", DateOfBirth = DateTime.Parse("1996-02-14"), Gender = "Female", Address = "2010 Farragut Pass", RecieveNewsLetter = true, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                persons.Add(new Person() { PersonID = Guid.Parse("7B75097B-BFF2-459F-8EA8-63742BBD7AFB"), PersonName = "Oby", Email = "oclutheram7@foxnews.com", DateOfBirth = DateTime.Parse("1992-05-31"), Gender = "Male", Address = "2 Fallview Plaza", RecieveNewsLetter = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                persons.Add(new Person() { PersonID = Guid.Parse("6717C42D-16EC-4F15-80D8-4C7413E250CB"), PersonName = "Seumas", Email = "ssimonitto8@biglobe.ne.jp", DateOfBirth = DateTime.Parse("1999-02-02"), Gender = "Male", Address = "76779 Norway Maple Crossing", RecieveNewsLetter = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                persons.Add(new Person() { PersonID = Guid.Parse("6E789C86-C8A6-4F18-821C-2ABDB2E95982"), PersonName = "Freemon", Email = "faugustin9@vimeo.com", DateOfBirth = DateTime.Parse("1996-04-27"), Gender = "Male", Address = "8754 Becker Street", RecieveNewsLetter = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

            }
        }

        public PersonResponse AddPerson(PersonAddRequest? person)
        {

            
            if(person is null)
                throw new ArgumentNullException(nameof(person));

            Person? personfromData = persons.FirstOrDefault(x=>x.Email == person.Email);

            if (personfromData != null)
                throw new ArgumentException("Email is Already Exist ");


            ValidationHelper.ModelValidation(person); 

            Person personToAdd = person.ToPerson();
            personToAdd.PersonID = Guid.NewGuid();

            persons.Add(personToAdd);

            return personToAdd.ToPersonResponse(); 
            
        }

        public bool DeletePerson(Guid? Id)
        {
            if (Id is null)
                return false;

            Person? person = persons.FirstOrDefault(x => x.PersonID == Id);

            if (person is null)
                return false;

            return persons.Remove(person); 

        }

        public List<PersonResponse> GetAllPersons()
        {
            List<PersonResponse> personResponses = new List<PersonResponse>();
            foreach (Person person in persons) {
                personResponses.Add(ConvertPersonToPersonResponse(person) );
            }

            return personResponses;
        }

        public List<PersonResponse> GetFilterdPersons(string SearchBy, string? SearchString)
        {
            if (string.IsNullOrEmpty(SearchString) || SearchBy is null)
                return GetAllPersons();

            List<Person> personsResult = new List<Person>(); 
            switch (SearchBy)
            {
                case nameof(PersonResponse.PersonName):
                    personsResult = persons.Where(x => x.PersonName.Contains(SearchString)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    personsResult = persons.Where(x => x.Email.Contains(SearchString)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    personsResult = persons.Where(x => (x.DateOfBirth != null)? x.DateOfBirth.Value.ToString().Contains(SearchString):true).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    personsResult = persons.Where(x => x.Gender.ToLower()==SearchString.ToLower()).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    personsResult = persons.Where(x => (x.Address != null) ? x.Address.Contains(SearchString):true).ToList();
                    break;
                case nameof(PersonResponse.CountryID):
                    personsResult = persons.Where(x => (x.CountryID != null) ? x.CountryID.ToString().Contains(SearchString) : true).ToList();
                    break;
            }


            List<PersonResponse> personResponses = new List<PersonResponse>();
            foreach (var item in personsResult)
            {
                personResponses.Add(ConvertPersonToPersonResponse(item));
            }
            return personResponses; 
        }

        public PersonResponse? GetPersonByPersonID(Guid? personId)
        {
            if (personId is null)
                return null;

            Person? person = persons.FirstOrDefault(x => x.PersonID == personId);


            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortedBy, SortOrderOptions sortOrder)
        {
            if (sortedBy is null)
                return persons; 

            List<PersonResponse> SortedPersons = new List<PersonResponse>();
            switch (sortedBy,sortOrder)
            {
                case (nameof(PersonResponse.PersonName),SortOrderOptions.ASC):
                    SortedPersons = persons.OrderBy(x => x.PersonName,StringComparer.OrdinalIgnoreCase).ToList(); 
                        break;
                case (nameof(PersonResponse.PersonName), SortOrderOptions.DESC):
                    SortedPersons = persons.OrderByDescending(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList();
                    break;
               
                case (nameof(PersonResponse.Email),SortOrderOptions.ASC):
                    SortedPersons = persons.OrderBy(x => x.Email).ToList(); 
                        break;
                case (nameof(PersonResponse.Email), SortOrderOptions.DESC):
                    SortedPersons = persons.OrderByDescending(x => x.Email).ToList();
                    break;

                case (nameof(PersonResponse.DateOfBirth),SortOrderOptions.ASC):
                    SortedPersons = persons.OrderBy(x => x.DateOfBirth).ToList(); 
                        break;
                case (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC):
                    SortedPersons = persons.OrderByDescending(x => x.
                    DateOfBirth).ToList();
                    break;

                case (nameof(PersonResponse.Age),SortOrderOptions.ASC):
                    SortedPersons = persons.OrderBy(x => x.Age).ToList(); 
                        break;
                case (nameof(PersonResponse.Age), SortOrderOptions.DESC):
                    SortedPersons = persons.OrderByDescending(x => x.
                    Age).ToList();
                    break;

                case (nameof(PersonResponse.Gender),SortOrderOptions.ASC):
                    SortedPersons = persons.OrderBy(x => x.Gender).ToList(); 
                        break;
                case (nameof(PersonResponse.Gender), SortOrderOptions.DESC):
                    SortedPersons = persons.OrderByDescending(x => x.
                    Gender).ToList();
                    break;

                case (nameof(PersonResponse.Address),SortOrderOptions.ASC):
                    SortedPersons = persons.OrderBy(x => x.Address).ToList(); 
                        break;
                case (nameof(PersonResponse.Address), SortOrderOptions.DESC):
                    SortedPersons = persons.OrderByDescending(x => x.
                    Address).ToList();
                    break;


                default:
                    SortedPersons = persons;
                    break; 
            }

            return SortedPersons; 

        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null )
                throw new ArgumentNullException("Invalid Data");

            if (personUpdateRequest.PersonName is null )
                throw new ArgumentException("Invalid Data");


            Person? person = persons.FirstOrDefault(x => x.PersonID == personUpdateRequest.PersonID);

            if (person is null)
                throw new ArgumentException("Invalid Person Id");


            person.PersonName = personUpdateRequest.PersonName; 
            person.Email = personUpdateRequest.Email; 
            person.DateOfBirth = personUpdateRequest.DateOfBirth; 
            person.Gender = personUpdateRequest.Gender.ToString(); 
            person.Address = personUpdateRequest.Address;
            person.CountryID = personUpdateRequest.CountryID;
            person.RecieveNewsLetter = personUpdateRequest.RecieveNewsLetter;



            return ConvertPersonToPersonResponse(person); 

        }




        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = countriesService.GetCountryByCountryId(person.CountryID)?.CountryName;
            return personResponse;
        }

    }
}
