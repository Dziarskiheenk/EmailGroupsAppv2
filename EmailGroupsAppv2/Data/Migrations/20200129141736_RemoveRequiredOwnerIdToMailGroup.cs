using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailGroupsAppv2.Data.Migrations
{
    public partial class RemoveRequiredOwnerIdToMailGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MailGroups_Name_OwnerId",
                table: "MailGroups");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "MailGroups",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_MailGroups_Name_OwnerId",
                table: "MailGroups",
                columns: new[] { "Name", "OwnerId" },
                unique: true,
                filter: "[OwnerId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MailGroups_Name_OwnerId",
                table: "MailGroups");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "MailGroups",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailGroups_Name_OwnerId",
                table: "MailGroups",
                columns: new[] { "Name", "OwnerId" },
                unique: true);
        }
    }
}
