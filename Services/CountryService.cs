using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly List<Country> _countries;
        public CountryService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                        _countries.AddRange(new List<Country>() {
                new Country() {  CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B"), CountryName = "USA" },

                new Country() { CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F"), CountryName = "Canada" },

                new Country() { CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"), CountryName = "UK" },

                new Country() { CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D"), CountryName = "India" },

                new Country() { CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB"), CountryName = "Australia" }
                });
            }
        }


        public CountryResponse AddCountry(CountryAddRequest? country)
        {
            // validation: check if country is null
            if (country is null)
            {
                throw new ArgumentNullException(nameof(country)); 
            }

            // validation : check if country name is null 
            if(country.CountryName is null)
            {
                throw new ArgumentException("Country name cannot be null", nameof(country.CountryName));
            }

            // validation : check if country name is dublicate

            if(_countries.Where(temp=> country.CountryName == temp.CountryName).Count()>0)
            {
                throw new ArgumentException("country Already exists", nameof(country.CountryName));
            }

            Country newCountry = country.ToCountry();

            newCountry.CountryID = Guid.NewGuid();

            _countries.Add(newCountry);
            return newCountry.ToCountryResponse(); 
        }

        public List<CountryResponse> GetAllCountries()
        {
           List<CountryResponse> CountriesResponse = new List<CountryResponse>();

            foreach(var country in _countries)
            {
                CountriesResponse.Add(country.ToCountryResponse()); 
            }

            return CountriesResponse;
        }

        public CountryResponse? GetCountryByCountryId(Guid?countryId)
        {
            if(countryId is null)
            {
                return null; 
            }

            Country? country = _countries.FirstOrDefault(x => x.CountryID == countryId);
            CountryResponse? countryResponse = country?.ToCountryResponse(); 

            return countryResponse;

        }
    }
}
