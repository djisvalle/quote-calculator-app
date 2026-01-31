using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteCalculator.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDecimalTo194 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumAmount",
                table: "ProductTypes",
                type: "numeric(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaximumAmount",
                table: "ProductTypes",
                type: "numeric(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                table: "ProductTypes",
                type: "numeric(5,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WeeklyRepayment",
                table: "LoanApplications",
                type: "numeric(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalRepayment",
                table: "LoanApplications",
                type: "numeric(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyRepayment",
                table: "LoanApplications",
                type: "numeric(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestFee",
                table: "LoanApplications",
                type: "numeric(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EstablishmentFee",
                table: "LoanApplications",
                type: "numeric(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "LoanApplications",
                type: "numeric(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumAmount",
                table: "ProductTypes",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaximumAmount",
                table: "ProductTypes",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                table: "ProductTypes",
                type: "numeric(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WeeklyRepayment",
                table: "LoanApplications",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalRepayment",
                table: "LoanApplications",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyRepayment",
                table: "LoanApplications",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestFee",
                table: "LoanApplications",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EstablishmentFee",
                table: "LoanApplications",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "LoanApplications",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)");
        }
    }
}
