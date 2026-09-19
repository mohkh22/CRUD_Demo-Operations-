using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest? person);
        List<PersonResponse> GetAllPersons();
        PersonResponse? GetPersonByPersonID(Guid? personId);

        List<PersonResponse> GetFilterdPersons(string SearchBy,string? SearchString); 
        List<PersonResponse> GetSortedPersons( List<PersonResponse> persons ,string sortedBy,SortOrderOptions sortOrder);

        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        bool DeletePerson(Guid? Id); 
    }
}
