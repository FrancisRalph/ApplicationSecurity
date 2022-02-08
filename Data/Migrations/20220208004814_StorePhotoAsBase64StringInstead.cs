using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationSecurity.Migrations
{
    public partial class StorePhotoAsBase64StringInstead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "AspNetUsers",
                newName: "Photo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "AspNetUsers",
                newName: "PhotoPath");
        }
    }
}
