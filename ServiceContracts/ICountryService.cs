using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        CountryResponse AddCountry(CountryAddRequest? country);

        List<CountryResponse> GetAllCountries();

        CountryResponse? GetCountryByCountryId(Guid?countryId); 
    }
}
