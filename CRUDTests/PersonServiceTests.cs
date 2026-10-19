using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonServiceTests
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper; 


        public PersonServiceTests(ITestOutputHelper testOutputHelper)
        {
            _countryService = new CountryService(new AppDbContext(new DbContextOptions<AppDbContext>()));
            _personService = new PersonService(new AppDbContext(new DbContextOptions<AppDbContext>()),_countryService);
            _testOutputHelper = testOutputHelper;
        }
        /*
         *  if addRequest is null 
         *  if email is already exist 
         *  if Valid data
         */


        #region AddPerson
        [Fact]
        public async Task AddPerson_PersonIsNull()
        {
            //Arrange
              PersonAddRequest? request = null;
            //Act and Assert
           await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.AddPerson(request);

            });    
        }


        [Fact]
        public async Task AddPerson_EmailAlreadyExist()
        {
            //Arrange
            PersonAddRequest request1 = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mo7amed.5aled171@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID=Guid.Parse("648F0BD1-BBF4-4876-90CC-3497D2C1E56C"),
                RecieveNewsLetter=true

            };

            PersonAddRequest request2 = new PersonAddRequest
            {
                PersonName = "Ahmed",
                Email = "mo7amed.5aled171@gmail.com",
                DateOfBirth = new DateTime(2006, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Cairo",
                CountryID = Guid.Parse("648F0BD1-BBF4-4876-90CC-3497D2C1E57E"),
                RecieveNewsLetter = false

            };

            _personService.AddPerson(request1);

            // Act and Assert

           await Assert.ThrowsAsync<ArgumentException>(async() =>{
                await _personService.AddPerson(request2);
            });
                
        }

        [Fact]
        public async Task AddPerson_ValidPerson()
        {
            //Arrange
            PersonAddRequest request = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mo7amed.5aled171@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = Guid.Parse("648F0BD1-BBF4-4876-90CC-3497D2C1E56C"),
                RecieveNewsLetter = true

            };
       
            // Act
            PersonResponse response = await _personService.AddPerson(request);

            // Assert
            Assert.True(response.PersonID != Guid.Empty); 
        }

        #endregion

        #region GetAllPersons

        [Fact]
        public async Task GetAllPersons_ListOfPersonsIsEmpty()
        {
            List<PersonResponse> persons = await _personService.GetAllPersons();
            Assert.Empty(persons);
        }

        [Fact]
        public async Task GetAllPersons_ListOfPersonsIsNotEmpty() {

            // Arrange
            PersonAddRequest request = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mo7amed.5aled171@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = Guid.Parse("648F0BD1-BBF4-4876-90CC-3497D2C1E56C"),
                RecieveNewsLetter = true

            };

            PersonResponse response = await _personService.AddPerson(request); 



            //Act
            List<PersonResponse> persons = await _personService.GetAllPersons();

            //Assert
            Assert.NotEmpty(persons);
        }

        #endregion


        #region GetPersonByPersonID 

        // if Person Id is null
        [Fact]
        public async Task GetPersonByPersonID_WithPersonIdIsNull()
        {
            Guid? id = null;

            PersonResponse? personResponse = await _personService.GetPersonByPersonID(id);

            Assert.Null(personResponse); 
        }

        // Get person with valid Person Id

        [Fact]
        public async Task GetPersonByPersonID_WithValidPersonId()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest personAdd = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mohamed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };


            PersonResponse personResponse = await _personService.AddPerson(personAdd);
            _testOutputHelper.WriteLine("Expexted: ");
            _testOutputHelper.WriteLine(personResponse.ToString()); 

            // Act

            PersonResponse? personFromData = await _personService.GetPersonByPersonID(personResponse.PersonID);
            _testOutputHelper.WriteLine("Actual: ");
            _testOutputHelper.WriteLine(personFromData?.ToString());

            // Assert

            Assert.Equal(personResponse, personFromData); 

        }


        #endregion

        #region GetFilteredPersons

        [Fact]
        public async Task GetFilteredPersons_SearchByPersonNameAndEmptySearchString()
        {
                //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest personAdd = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mohamed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonAddRequest personAdd1 = new PersonAddRequest
            {
                PersonName = "Ahmed",
                Email = "Ahmed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonAddRequest personAdd2 = new PersonAddRequest
            {
                PersonName = "Omar",
                Email = "Omar@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };


            _personService.AddPerson(personAdd);
            _personService.AddPerson(personAdd1);
            _personService.AddPerson(personAdd2);

            List<PersonResponse> PersonFromGetAll = await _personService.GetAllPersons(); 

            //Act
            List<PersonResponse> personFromSearch = await _personService.GetFilterdPersons(nameof(Person.PersonName), "");

            // Assert

             Assert.Equal(personFromSearch.Count, PersonFromGetAll.Count);
        }


        [Fact]
        public async Task GetFilteredPersons_SearchByPersonNameAndSearchString()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest personAdd = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mohamed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonAddRequest personAdd1 = new PersonAddRequest
            {
                PersonName = "Ahmed",
                Email = "Ahmed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonAddRequest personAdd2 = new PersonAddRequest
            {
                PersonName = "Omar",
                Email = "Omar@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>(); 

            personResponsesFromAdd.Add(await _personService.AddPerson(personAdd));
            personResponsesFromAdd.Add(await _personService.AddPerson(personAdd1));
            personResponsesFromAdd.Add(await _personService.AddPerson(personAdd2));

            //Act

            List<PersonResponse> personFromSearch = await _personService.GetFilterdPersons(nameof(Person.PersonName), "me");

            // Assert

            foreach (var item in personFromSearch)
            {
                if(item is not null)
                {
                    if (item.PersonName !=null &&item.PersonName.Contains("ma",StringComparison.OrdinalIgnoreCase)){

                       Assert.Contains(item, personResponsesFromAdd); 
                    }
                }
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest personAdd = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mohamed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonAddRequest personAdd1 = new PersonAddRequest
            {
                PersonName = "Ahmed",
                Email = "Ahmed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonAddRequest personAdd2 = new PersonAddRequest
            {
                PersonName = "Omar",
                Email = "Omar@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();

            personResponsesFromAdd.Add(await _personService.AddPerson(personAdd));
            personResponsesFromAdd.Add(await _personService.AddPerson(personAdd1));
            personResponsesFromAdd.Add(await _personService.AddPerson(personAdd2));

            //Act

            List<PersonResponse> PersonsFromGetAll = await _personService.GetAllPersons();
            List<PersonResponse> ActualSortedPersons = PersonsFromGetAll.OrderByDescending(x => x.PersonName).ToList() ; 
           

            List<PersonResponse> SortedPersons =   _personService.GetSortedPersons(PersonsFromGetAll,nameof(Person.PersonName), SortOrderOptions.DESC);

            // Assert

            for (int i = 0; i < personResponsesFromAdd.Count; i++)
            {
                Assert.Equal(ActualSortedPersons[i], SortedPersons[i]); 
            }
        }
        #endregion

        #region Update Person 
        // if person Request is null 

        [Fact]
        public async Task UpdatePerson_WithPersonIsNull()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>{
                await _personService.UpdatePerson(personUpdateRequest); 
            }); 
        }

        // invalid Person Id 
        [Fact]
        public async Task UpdatePerson_WithInvalidPersonID()
        {
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = Guid.NewGuid()
            }; 

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }
        // person name is null 

        [Fact]
        public async Task UpdatePerson_WithPersonNameIsNull()
        {
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Egypt"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest request = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mo7amed.5aled171@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = Guid.Parse("648F0BD1-BBF4-4876-90CC-3497D2C1E56C"),
                RecieveNewsLetter = true

            };

            PersonResponse personResponse = await _personService.AddPerson(request); 


            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = personResponse.PersonID,
                PersonName= null
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }
        // Update person with valid Details  
        [Fact]
        public async Task UpdatePerson_WithValidDetails()
        {
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Egypt"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest request = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mo7amed.5aled171@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = Guid.Parse("648F0BD1-BBF4-4876-90CC-3497D2C1E56C"),
                RecieveNewsLetter = true

            };

            PersonResponse personResponse = await _personService.AddPerson(request);


            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = personResponse.PersonID,
                PersonName = "Ahmed",
                Email = "Ahmed.5aled171@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            //Act

            PersonResponse personResponseFromUpdate = await _personService.UpdatePerson(personUpdateRequest); 
            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonID(personResponse.PersonID);

            //Assert

            Assert.Equal(personResponseFromUpdate, personResponseFromGet); 
        }
        #endregion


        #region Delete Person 

        // Person id is null
        [Fact]
        public async Task DeletePerson_WithPersonIdIsNull()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest personAdd = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mohamed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonResponse personResponse = await _personService.AddPerson(personAdd);

            bool IsDeleted = await _personService.DeletePerson(null);

            Assert.False(IsDeleted);



        }

        // Invaid Person Id 
        [Fact]
        public async Task DeletePerson_WithInvalidPersonId()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest personAdd = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mohamed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonResponse personResponse = await _personService.AddPerson(personAdd);

            bool IsDeleted = await _personService.DeletePerson(Guid.NewGuid());

            Assert.False(IsDeleted);

        }

        // Valid Person Id 
        [Fact]
        public async Task DeletePerson_WithValidPersonId()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = await _countryService.AddCountry(countryAdd);

            PersonAddRequest personAdd = new PersonAddRequest
            {
                PersonName = "Mohamed",
                Email = "mohamed@gmail.com",
                DateOfBirth = new DateTime(2004, 9, 1),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                Address = "Minia",
                CountryID = countryResponse.CountryID,
                RecieveNewsLetter = true
            };

            PersonResponse  personResponse =await _personService.AddPerson(personAdd);

            bool IsDeleted = await _personService.DeletePerson(personResponse.PersonID);

            Assert.True(IsDeleted); 

           
        }
        #endregion

    }

}
