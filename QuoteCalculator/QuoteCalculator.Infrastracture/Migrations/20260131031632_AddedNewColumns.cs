using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteCalculator.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalInterest",
                table: "LoanApplications",
                newName: "WeeklyRepayment");

            migrationBuilder.AddColumn<decimal>(
                name: "InterestFee",
                table: "LoanApplications",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyRepayment",
                table: "LoanApplications",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "LoanApplications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestFee",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "MonthlyRepayment",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "LoanApplications");

            migrationBuilder.RenameColumn(
                name: "WeeklyRepayment",
                table: "LoanApplications",
                newName: "TotalInterest");
        }
    }
}
