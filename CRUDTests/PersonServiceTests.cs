using Entities;
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
            _personService = new PersonService(false); 
            _countryService = new CountryService(false);
            _testOutputHelper = testOutputHelper;
        }
        /*
         *  if addRequest is null 
         *  if email is already exist 
         *  if Valid data
         */


        #region AddPerson
        [Fact]
        public void AddPerson_PersonIsNull()
        {
            //Arrange
              PersonAddRequest? request = null;
            //Act and Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(request);

            });    
        }


        [Fact]
        public void AddPerson_EmailAlreadyExist()
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

            Assert.Throws<ArgumentException>(() =>{
                _personService.AddPerson(request2);
            });
                
        }

        [Fact]
        public void AddPerson_ValidPerson()
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
            PersonResponse response = _personService.AddPerson(request);

            // Assert
            Assert.True(response.PersonID != Guid.Empty); 
        }

        #endregion

        #region GetAllPersons

        [Fact]
        public void GetAllPersons_ListOfPersonsIsEmpty()
        {
            List<PersonResponse> persons = _personService.GetAllPersons();
            Assert.Empty(persons);
        }

        [Fact]
        public void GetAllPersons_ListOfPersonsIsNotEmpty() {

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

            PersonResponse response = _personService.AddPerson(request); 



            //Act
            List<PersonResponse> persons = _personService.GetAllPersons();

            //Assert
            Assert.NotEmpty(persons);
        }

        #endregion


        #region GetPersonByPersonID 

        // if Person Id is null
        [Fact]
        public void GetPersonByPersonID_WithPersonIdIsNull()
        {
            Guid? id = null;

            PersonResponse? personResponse = _personService.GetPersonByPersonID(id);

            Assert.Null(personResponse); 
        }

        // Get person with valid Person Id

        [Fact]
        public void GetPersonByPersonID_WithValidPersonId()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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


            PersonResponse personResponse = _personService.AddPerson(personAdd);
            _testOutputHelper.WriteLine("Expexted: ");
            _testOutputHelper.WriteLine(personResponse.ToString()); 

            // Act

            PersonResponse? personFromData = _personService.GetPersonByPersonID(personResponse.PersonID);
            _testOutputHelper.WriteLine("Actual: ");
            _testOutputHelper.WriteLine(personFromData?.ToString());

            // Assert

            Assert.Equal(personResponse, personFromData); 

        }


        #endregion

        #region GetFilteredPersons

        [Fact]
        public void GetFilteredPersons_SearchByPersonNameAndEmptySearchString()
        {
                //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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

            List<PersonResponse> PersonFromGetAll = _personService.GetAllPersons(); 

            //Act
            List<PersonResponse> personFromSearch = _personService.GetFilterdPersons(nameof(Person.PersonName), "");

            // Assert

             Assert.Equal(personFromSearch.Count, PersonFromGetAll.Count);
        }


        [Fact]
        public void GetFilteredPersons_SearchByPersonNameAndSearchString()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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

            personResponsesFromAdd.Add( _personService.AddPerson(personAdd));
            personResponsesFromAdd.Add( _personService.AddPerson(personAdd1));
            personResponsesFromAdd.Add( _personService.AddPerson(personAdd2));

            //Act

            List<PersonResponse> personFromSearch = _personService.GetFilterdPersons(nameof(Person.PersonName), "me");

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
        public void GetSortedPersons()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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

            personResponsesFromAdd.Add(_personService.AddPerson(personAdd));
            personResponsesFromAdd.Add(_personService.AddPerson(personAdd1));
            personResponsesFromAdd.Add(_personService.AddPerson(personAdd2));

            //Act

            List<PersonResponse> PersonsFromGetAll = _personService.GetAllPersons();
            List<PersonResponse> ActualSortedPersons = PersonsFromGetAll.OrderByDescending(x => x.PersonName).ToList() ; 
           

            List<PersonResponse> SortedPersons = _personService.GetSortedPersons(PersonsFromGetAll,nameof(Person.PersonName), SortOrderOptions.DESC);

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
        public void UpdatePerson_WithPersonIsNull()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            Assert.Throws<ArgumentNullException>( () =>{
                _personService.UpdatePerson(personUpdateRequest); 
            }); 
        }

        // invalid Person Id 
        [Fact]
        public void UpdatePerson_WithInvalidPersonID()
        {
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = Guid.NewGuid()
            }; 

            Assert.Throws<ArgumentException>(() => {
                _personService.UpdatePerson(personUpdateRequest);
            });
        }
        // person name is null 

        [Fact]
        public void UpdatePerson_WithPersonNameIsNull()
        {
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Egypt"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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

            PersonResponse personResponse = _personService.AddPerson(request); 


            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = personResponse.PersonID,
                PersonName= null
            };

            Assert.Throws<ArgumentException>(() => {
                _personService.UpdatePerson(personUpdateRequest);
            });
        }
        // Update person with valid Details  
        [Fact]
        public void UpdatePerson_WithValidDetails()
        {
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Egypt"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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

            PersonResponse personResponse = _personService.AddPerson(request);


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

            PersonResponse personResponseFromUpdate = _personService.UpdatePerson(personUpdateRequest); 
            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonID(personResponse.PersonID);

            //Assert

            Assert.Equal(personResponseFromUpdate, personResponseFromGet); 
        }
        #endregion


        #region Delete Person 

        // Person id is null
        [Fact]
        public void DeletePerson_WithPersonIdIsNull()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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

            PersonResponse personResponse = _personService.AddPerson(personAdd);

            bool IsDeleted = _personService.DeletePerson(null);

            Assert.False(IsDeleted);



        }

        // Invaid Person Id 
        [Fact]
        public void DeletePerson_WithInvalidPersonId()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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

            PersonResponse personResponse = _personService.AddPerson(personAdd);

            bool IsDeleted = _personService.DeletePerson(Guid.NewGuid());

            Assert.False(IsDeleted);

        }

        // Valid Person Id 
        [Fact]
        public void DeletePerson_WithValidPersonId()
        {
            //Arrange 
            CountryAddRequest countryAdd = new CountryAddRequest
            {
                CountryName = "Minia"
            };

            CountryResponse countryResponse = _countryService.AddCountry(countryAdd);

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

            PersonResponse  personResponse =_personService.AddPerson(personAdd);

            bool IsDeleted = _personService.DeletePerson(personResponse.PersonID);

            Assert.True(IsDeleted); 

           
        }
        #endregion

    }

}
