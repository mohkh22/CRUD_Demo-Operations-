using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUD.Controllers
{
    public class PersonsController : Controller
    {

        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;

        public PersonsController(IPersonService personService, ICountryService countryService)
        {
            _personService = personService;
            _countryService = countryService;
        }

        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string SearchBy,
                                string? SearchString,
                                string SortBy,
                                SortOrderOptions SortOrder)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName),"Person Name" },
                {nameof(PersonResponse.Email),"Email" },
                {nameof(PersonResponse.DateOfBirth),"Date Of Birth" },
                {nameof(PersonResponse.CountryID),"Country" },
                {nameof(PersonResponse.Address),"Address" },
                {nameof(PersonResponse.Gender),"Gender" },
            };

            
            
            List<PersonResponse> persons = _personService.GetFilterdPersons(SearchBy,SearchString);
            ViewBag.CurrentSearchString = SearchString;
            ViewBag.CurrentSearchBy = SearchBy;
            List<PersonResponse> SortedPersons = _personService.GetSortedPersons(persons, SortBy, SortOrder);
            ViewBag.CurrentSortBy = SortBy;
            ViewBag.CurrentSortOrder = SortOrder;
            return View(SortedPersons);
        }


        [Route("persons/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = _countryService.GetAllCountries();
            return View(); 
        }


        [HttpPost]
        [Route("persons/create")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countryService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            //call the service method
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            //navigate to Index() action method (it makes another get request to "persons/index"
            return RedirectToAction("Index", "Persons");
        }



    }


    
}
