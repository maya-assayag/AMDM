using Microsoft.EntityFrameworkCore.Migrations;

namespace AMDM.Migrations
{
    public partial class ticketTypeEnumProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketPeriod",
                table: "TicketType",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketPeriod",
                table: "TicketType");
        }
    }
}
