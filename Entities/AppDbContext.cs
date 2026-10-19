using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");


            string CountriesJson = File.ReadAllText("countries.json");

            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(CountriesJson);

            foreach (var item in countries)
            {
            modelBuilder.Entity<Country>().HasData(item);

            }


            string PersonsJson = File.ReadAllText("persons.json");

            List<Person> people = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(PersonsJson);


            foreach(var person in people)
            {
                modelBuilder.Entity<Person>().HasData(person); 
            }

        }

        public Task<List<Person>> GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToListAsync();
        }
    }
}
