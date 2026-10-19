using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        Task<CountryResponse> AddCountry(CountryAddRequest? country);

        Task<List<CountryResponse>> GetAllCountries();

        Task<CountryResponse?> GetCountryByCountryId(Guid?countryId); 
    }
}
