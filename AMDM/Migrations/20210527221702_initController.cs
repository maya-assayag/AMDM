using Microsoft.EntityFrameworkCore.Migrations;

namespace AMDM.Migrations
{
    public partial class initController : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TraineeGender",
                table: "Trainer",
                newName: "TrainerGender");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainerGender",
                table: "Trainer",
                newName: "TraineeGender");
        }
    }
}
