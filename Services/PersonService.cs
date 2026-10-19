using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICountryService countriesService;

        public PersonService(AppDbContext appDbContext , ICountryService countryService)
        {
            _dbContext = appDbContext;
            countriesService = countryService;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? person)
        {    
            if(person is null)
                throw new ArgumentNullException(nameof(person));

            Person? personfromData = await _dbContext.Persons.FirstOrDefaultAsync(x=>x.Email == person.Email);

            if (personfromData != null)
                throw new ArgumentException("Email is Already Exist ");


            ValidationHelper.ModelValidation(person); 

            Person personToAdd = person.ToPerson();
            personToAdd.PersonID = Guid.NewGuid();

          await  _dbContext.Persons.AddAsync(personToAdd);
            await _dbContext.SaveChangesAsync();

            return personToAdd.ToPersonResponse(); 
            
        }

        public async Task<bool> DeletePerson(Guid? Id)
        {
            if (Id is null)
                return false;

            Person? person = await _dbContext.Persons.FirstOrDefaultAsync(x => x.PersonID == Id);

            if (person is null)
                return false;

            _dbContext.Persons.Remove(person);
            return await _dbContext.SaveChangesAsync() > 0;

        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            List<PersonResponse> personResponses = new List<PersonResponse>();
            var persons = await _dbContext.Persons.Include(p => p.Country).ToListAsync(); 
            foreach (Person person in persons) {
                personResponses.Add(ConvertPersonToPersonResponse(person) );
            }

            return personResponses;
        }

        public async Task<List<PersonResponse>> GetFilterdPersons(string SearchBy, string? SearchString)
        {
            if (string.IsNullOrEmpty(SearchString) || SearchBy is null)
                return await GetAllPersons();

            List<Person> personsResult = new List<Person>(); 
            switch (SearchBy)
            {
                case nameof(PersonResponse.PersonName):
                    personsResult = await _dbContext.Persons.Where(x => x.PersonName.Contains(SearchString)).ToListAsync();
                    break;
                case nameof(PersonResponse.Email):
                    personsResult = await _dbContext.Persons.Where(x => x.Email.Contains(SearchString)).ToListAsync();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    personsResult = await _dbContext.Persons.Where(x => (x.DateOfBirth != null)? x.DateOfBirth.Value.ToString().Contains(SearchString):true).ToListAsync();
                    break;
                case nameof(PersonResponse.Gender):
                    personsResult = await _dbContext.Persons.Where(x => x.Gender.ToLower()==SearchString.ToLower()).ToListAsync();
                    break;
                case nameof(PersonResponse.Address):
                    personsResult = await _dbContext.Persons.Where(x => (x.Address != null) ? x.Address.Contains(SearchString):true).ToListAsync();
                    break;
                case nameof(PersonResponse.CountryID):
                    personsResult = await _dbContext.Persons.Where(x => (x.CountryID != null) ? x.CountryID.ToString().Contains(SearchString) : true).ToListAsync();
                    break;
            }


            List<PersonResponse> personResponses = new List<PersonResponse>();
            foreach (var item in personsResult)
            {
                personResponses.Add(ConvertPersonToPersonResponse(item));
            }
            return personResponses; 
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personId)
        {
            if (personId is null)
                return null;

            Person? person = await _dbContext.Persons.FirstOrDefaultAsync(x => x.PersonID == personId);

            if (person is null)
                return null;

            return ConvertPersonToPersonResponse(person);
        }

        public async Task<MemoryStream> GetPersonCSV()
        {
            MemoryStream memoryStream = new MemoryStream(); 
            StreamWriter streamWriter = new StreamWriter(memoryStream);
             CsvWriter csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture,leaveOpen:true);

            csvWriter.WriteHeader<PersonResponse>();
            csvWriter.NextRecord();

            List<PersonResponse> persons = _dbContext.Persons
                .Include(x=>x.Country)
                .Select(x => x.ToPersonResponse()).ToList();
            csvWriter.WriteRecords(persons);

            memoryStream.Position = 0;

            return memoryStream; 

        }

        public  List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortedBy, SortOrderOptions sortOrder)
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null )
                throw new ArgumentNullException("Invalid Data");

            if (personUpdateRequest.PersonName is null )
                throw new ArgumentException("Invalid Data");


            Person? person = await _dbContext.Persons.FirstOrDefaultAsync(x => x.PersonID == personUpdateRequest.PersonID);

            if (person is null)
                throw new ArgumentException("Invalid Person Id");


            person.PersonName = personUpdateRequest.PersonName; 
            person.Email = personUpdateRequest.Email; 
            person.DateOfBirth = personUpdateRequest.DateOfBirth; 
            person.Gender = personUpdateRequest.Gender.ToString(); 
            person.Address = personUpdateRequest.Address;
            person.CountryID = personUpdateRequest.CountryID;
            person.RecieveNewsLetter = personUpdateRequest.RecieveNewsLetter;

            await _dbContext.SaveChangesAsync();


            return ConvertPersonToPersonResponse(person); 

        }




        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            return personResponse;
        }

    }
}
