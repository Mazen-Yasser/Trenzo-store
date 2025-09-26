using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrenzoStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesForViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressLine2",
                table: "Orders",
                newName: "ShippingAddress2");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressLine1",
                table: "Orders",
                newName: "ShippingAddress1");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Addresses",
                newName: "AddressType");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Addresses",
                newName: "ZipCode");

            migrationBuilder.RenameColumn(
                name: "AddressLine2",
                table: "Addresses",
                newName: "Address1");

            migrationBuilder.AddColumn<string>(
                name: "PaymentDetails",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingPhone",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Addresses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Addresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Addresses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDetails",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingPhone",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress2",
                table: "Orders",
                newName: "ShippingAddressLine2");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress1",
                table: "Orders",
                newName: "ShippingAddressLine1");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Addresses",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "AddressType",
                table: "Addresses",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Address1",
                table: "Addresses",
                newName: "AddressLine2");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Addresses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
