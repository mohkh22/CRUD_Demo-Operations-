using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetAllPersons_storedProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"Create Procedure [dbo].[GetAllPersons]
                                        as begin
                                            select * from Persons
                                        end";
            migrationBuilder.Sql(sp_GetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"Drop Procedure [dbo].[GetAllPersons] ";
            migrationBuilder.Sql(sp_GetAllPersons);
        }
    }
}
