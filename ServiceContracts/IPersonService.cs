using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? person);
        Task<List<PersonResponse>> GetAllPersons();
        Task<PersonResponse?> GetPersonByPersonID(Guid? personId);

        Task<List<PersonResponse>> GetFilterdPersons(string SearchBy,string? SearchString); 
        List<PersonResponse> GetSortedPersons( List<PersonResponse> persons ,string sortedBy,SortOrderOptions sortOrder);

        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        Task<bool> DeletePerson(Guid? Id);

        Task<MemoryStream> GetPersonCSV(); 
    }
}
