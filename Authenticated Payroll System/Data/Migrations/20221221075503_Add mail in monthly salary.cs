using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authenticated_Payroll_System.Data.Migrations
{
    public partial class Addmailinmonthlysalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MonthlySalary",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "MonthlySalary");
        }
    }
}
