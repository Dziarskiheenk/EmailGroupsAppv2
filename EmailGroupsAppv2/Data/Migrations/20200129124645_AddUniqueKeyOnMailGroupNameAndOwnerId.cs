using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailGroupsAppv2.Data.Migrations
{
    public partial class AddUniqueKeyOnMailGroupNameAndOwnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MailGroups_Name",
                table: "MailGroups");

            migrationBuilder.CreateIndex(
                name: "IX_MailGroups_Name_OwnerId",
                table: "MailGroups",
                columns: new[] { "Name", "OwnerId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MailGroups_Name_OwnerId",
                table: "MailGroups");

            migrationBuilder.CreateIndex(
                name: "IX_MailGroups_Name",
                table: "MailGroups",
                column: "Name",
                unique: true);
        }
    }
}
