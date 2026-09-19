using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountryServiceTests
    {
        private readonly ICountryService _countryService;

        public CountryServiceTests()
        {
            _countryService = new CountryService(false);
        }

        #region AddCountry 
        // if CountryAddRequest is null
        //  throw ArgumentNullException

        [Fact]
        public void AddCountry_NullCountryAddRequest()
        {
            // Arrange 
              CountryAddRequest? request = null;

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() =>{
                _countryService.AddCountry(request); 
            }); 
        }

        // If CountryName is Null , throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest()
            {
                CountryName = null
            };
            //Act and Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countryService.AddCountry(request);
            });
        }

        // If CountryName is Dublicate , throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            //Act and Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countryService.AddCountry(request1);
                _countryService.AddCountry(request2);
            });
        }

        // If CountryName is Valid ,
        // return CountryResponse object with CountryId and CountryName
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            // Arrange 
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };

            // Act 
            CountryResponse  response= _countryService.AddCountry(request);
            List<CountryResponse> result = _countryService.GetAllCountries(); 

            // Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, result); 
        }

        #endregion

        #region GetAllCountries
        // if no countries are added, return empty list
        [Fact]
        public void GetAllCountries_CountryListIsEmpty()
        {
            //Acts 
            List<CountryResponse> response = _countryService.GetAllCountries(); 

            //Assert
            Assert.Empty(response);
        }

        // if return countries 
        [Fact]
        public void GetAllCountries_AddFewCountries() 
        {

            //Assert
            List<CountryAddRequest> CountriesRequest = new List<CountryAddRequest>()
            {
                new CountryAddRequest{CountryName="Egypt"},
                new CountryAddRequest{CountryName="USA"}
            };

            List<CountryResponse> CountriesResponseFromAdd = new List<CountryResponse>(); 

            foreach (CountryAddRequest request in CountriesRequest)
            {
                CountriesResponseFromAdd.Add(_countryService.AddCountry(request)); 
            }

            // Act
            List<CountryResponse> actualCountriesResponse = _countryService.GetAllCountries(); 
            
            // Assert
            foreach(CountryResponse expected in CountriesResponseFromAdd)
            {
                Assert.Contains(expected,actualCountriesResponse); 
            }
        }

        #endregion


        #region GetCountryByCountryId
        [Fact]
        public void GetCountryByCountryId_CountryIdIsNull()
        {
            // Arange 
            Guid? countryId = null;

            // Act 
            CountryResponse? response = _countryService.GetCountryByCountryId(countryId);

            // Assert
            Assert.Null(response);
            
        }

        [Fact]
        public void GetCountryByCountryId_CountryIdIsValid()
        {
            // Arrange 
            CountryAddRequest request = new CountryAddRequest()
            {
                CountryName = "China"
            };

            CountryResponse response = _countryService.AddCountry(request);

            // Act
            CountryResponse? Country = _countryService.GetCountryByCountryId(response.CountryID);

            // Assert 
            Assert.Equal(response, Country);  
        }
        
        #endregion


    }
}
