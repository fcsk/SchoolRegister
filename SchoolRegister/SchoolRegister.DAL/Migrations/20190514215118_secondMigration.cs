using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolRegister.DAL.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Subjects_SubjectId",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherId",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Subjects",
                newName: "TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                newName: "IX_Subjects_TeacherID");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherID",
                table: "Subjects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Grade",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectGroup",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId1 = table.Column<int>(nullable: true),
                    SubjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectGroup", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_SubjectGroup_Groups_GroupId1",
                        column: x => x.GroupId1,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectGroup_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectGroup_GroupId1",
                table: "SubjectGroup",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectGroup_SubjectId",
                table: "SubjectGroup",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Subjects_SubjectId",
                table: "Grade",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherID",
                table: "Subjects",
                column: "TeacherID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Subjects_SubjectId",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherID",
                table: "Subjects");

            migrationBuilder.DropTable(
                name: "SubjectGroup");

            migrationBuilder.RenameColumn(
                name: "TeacherID",
                table: "Subjects",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_TeacherID",
                table: "Subjects",
                newName: "IX_Subjects_TeacherId");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Subjects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Grade",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Subjects_SubjectId",
                table: "Grade",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
