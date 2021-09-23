using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskMasterTutorial.Migrations
{
    public partial class AddStatusSeedDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adds initial values in our Statuses Table.
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new string[] { "Name" },
                values: new object[,]
                {
                    {"To Do"},
                    {"In Progress" },
                    {"Done"}
                }
            );
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback the added values if needed.
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Name",
                keyValues: new object[] {
                    "To Do",
                    "In Progress",
                    "Done"
                }
            );
        }
        
    }
}
