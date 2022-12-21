using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authenticated_Payroll_System.Data.Migrations
{
    public partial class addisapproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "LeaveApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "LeaveApplications");
        }
    }
}
