using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authenticated_Payroll_System.Data.Migrations
{
    public partial class filedchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "isApproved",
                table: "LeaveApplications",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isApproved",
                table: "LeaveApplications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
