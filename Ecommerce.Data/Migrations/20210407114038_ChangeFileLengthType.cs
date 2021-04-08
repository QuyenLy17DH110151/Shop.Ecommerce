using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Data.Migrations
{
    public partial class ChangeFileLengthType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f7de0ae-be1b-4e92-9ca7-32a11f8c5ee1"),
                column: "ConcurrencyStamp",
                value: "ebddeabe-3b13-4d7a-be1c-e4e0d17cc330");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("fadf5757-d5aa-476e-b7df-f1bfcca313d0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6fc9b382-31ed-4230-9527-a8386a74e29a", "AQAAAAEAACcQAAAAEP//ayVkKs1QVgBPkbtvaQmeF39gCs7ckoBvyVddW5kBrDswTvLLu5xWUcJ7j7lB6w==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 7, 18, 40, 37, 257, DateTimeKind.Local).AddTicks(1942));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f7de0ae-be1b-4e92-9ca7-32a11f8c5ee1"),
                column: "ConcurrencyStamp",
                value: "b74c422a-ee7b-4e1d-8e40-c1e390ac1dc9");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("fadf5757-d5aa-476e-b7df-f1bfcca313d0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ca494ee2-fda8-4446-8a72-539f38333860", "AQAAAAEAACcQAAAAELZ/IUv2MFpInewZMJawy8CyPJZhq6964fQBCUQ7VbeoCH6n9UqMta+KX8+2soBI8w==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 4, 17, 2, 4, 299, DateTimeKind.Local).AddTicks(3533));
        }
    }
}
