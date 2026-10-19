using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly AppDbContext _dbContext;
        public CountryService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task<CountryResponse> AddCountry(CountryAddRequest? country)
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

            if(await _dbContext.Countries.Where(temp=> country.CountryName == temp.CountryName).CountAsync()>0)
            {
                throw new ArgumentException("country Already exists", nameof(country.CountryName));
            }

            Country newCountry = country.ToCountry();

            newCountry.CountryID = Guid.NewGuid();

            _dbContext.Countries.Add(newCountry);
            await _dbContext.SaveChangesAsync();
            return newCountry.ToCountryResponse(); 
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
           List<CountryResponse> CountriesResponse = new List<CountryResponse>();
            var countries = await _dbContext.Countries.ToListAsync();
            foreach (var country in countries)
            {
                CountriesResponse.Add(country.ToCountryResponse()); 
            }

            return CountriesResponse;
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid?countryId)
        {
            if(countryId is null)
            {
                return null; 
            }

            Country? country = await _dbContext.Countries.FirstOrDefaultAsync(x => x.CountryID == countryId);
            CountryResponse? countryResponse = country?.ToCountryResponse(); 

            return countryResponse;

        }
    }
}
