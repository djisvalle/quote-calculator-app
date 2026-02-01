# Quote Calculator App

A high-precision quote calculator application built with .NET 10, React, and PostgreSQL. This project calculates loan repayments (Weekly, Monthly, Total) and interest fees using 4-decimal precision, specifically calibrated to match **Excel PMT** logic.

## Projects in this Solution

| Project  | Type | Description
| ------------- | ------------- | ------------- |
| **QuoteCalculator.API**  | Web API  | **Main Entry Point**. Run to start the API.
| **QuoteCalculator.Application** | Class Library  | Contains business logic and DTOs
| **QuoteCalculator.Domain** | Class Library  | Contains Entities and repository interfaces
| **QuoteCalculator.Infrastracture** | Class Library  | Handles database context via EF Core and Repository implementations
| **QuoteCalculator.Test** | xUnit v3  | Contains unit tests to verify calculations
| **quote-calculator-app** | React Web Application  | User interface for the Quote Calculator

## Getting Started

**1. Database Setup (PostgreSQL)**

This project uses Entity Framework Core with Postgres. Follow these steps to restore the database schema and migration data:

1. **Update Connection String**: Open `appsettings.json` in the **QuoteCalculator.API** project and update the `QuoteCalculatorDatabase`:

    ```
    "ConnectionStrings": {
        "QuoteCalculatorDatabase": "Server=localhost;Port=5432;Database=QuoteCalculator;User Id=user;Password=password;"
    }
    ```

2. **Restore Database**: Open your terminal in the root folder and run:
   
    ```
    dotnet ef database update --project QuoteCalculator.Infrastracture --startup-project QuoteCalculator.API
    ```

**2. Running the application**

**Option A: Run via Visual Studio (Recommended)**
  
This is the most straightforward way to run the solution.
1. Open the `QuoteCalculator.sln` file
2. Ensure `QuoteCalculator.API` is set as the **Startup Project**
3. Press **F5** or the **Play** button to start debugging.
4. A console will open that will inform you that the API is running.

   * _For this project, Swagger is not enabled._

**Option B: Run via CLI**

Use this if you are on VS Code, or you don't have Visual Studio installed.
1. Open your terminal/command prompt.
2. Navigate to the API project folder:

    ```
    cd \QuoteCalculator\QuoteCalculator.API
    ```

3. Run the application:

    ```
    dotnet run
    ```

4. The application will start (at `http://localhost:5271`).

## Testing

This solution uses **xUnit v3**. Note that v3 test projects are executables.

**To run tests via CLI**:

    dotnet test

## API Endpoints

**1. Get Application URL**

Saves initial customer data and returns a URL that will redirect the applicant to the Quote Calculator App.

**URL**: 

```
POST /api/loan
```

**Payload**:

```
{
  "AmountRequired":"5000",
  "Term":"2",
  "Title":"Mr.",
  "FirstName":"John",
  "LastName": "Doe",
  "DateOfBirth":"1900-01-01",
  "Mobile":"0422111333",
  "Email":"layton@gmail.com"
}
```

**Response (200 OK)**

```
"http://localhost:5173/quote-calculator/initial-quote?loans=7d1dce08-a93a-468f-8577-1ff902a800c2"
```

**2. Calculate Loan Details**

Calculates the monthly, weekly, and total repayments based on product type, amount, and other fees.

**URL**: 

```
POST /api/loan/calculate
```

**Payload**:

```
{
  "productTypeId": 2,
  "amount": 5000,
  "term": 24
}
```

**Response (200 OK)**

```
{
  "amount": 5000,
  "term": 24,
  "establishmentFee": 300,
  "interestFee": 511.9216,
  "weeklyRepayment": 55.8839,
  "monthlyRepayment": 242.1634,
  "totalRepayment": 5811.9216
}
```

**3. Get Product Types**

Retrieves available loan product types with different interest rates, terms, and promos.

**URL**: 

```
GET /api/producttype
```

## Calculation and Precision

- **Precision**: Internal math is performed at 16-byte `decimal` precision and rounded to 4 **decimal places** for both storage and DTOs.
- **Verification**: Verified calculated PMT against Excel PMT to ensure accurate computations, with unit tests having an approximate difference of 0.0001.
- **Rounding**: Uses `MidpointRound.AwayFromZero` to ensure consistency between payments and totals.
- **Weekly Logic**: Annualized via _(Total / (Term x 4.3333))_ to account for year-long averages.
