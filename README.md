# KarenderyaWebApp

A web-based Karenderya Management System developed using ASP.NET Core Razor Pages and Microsoft SQL Server. The application provides basic food inventory management and order processing features with separate access for administrators and cashiers.

## Features

- **User Authentication** — Login and logout using session-based authentication.
- **Role-Based Access** — Separate access for Admin and Cashier roles.
- **Account Activation** — Account activation checks to restrict access for inactive accounts.
- **Food Management** — Manage food items, prices, availability, and stock quantities.
- **Order Processing** — Create customer orders and calculate order totals.
- **Inventory Updates** — Automatically deduct available stock when an order is completed.
- **Order Confirmation** — Display a success modal after a successful order.
- **Order History** — View recent transactions and inspect the items included in each order.
- **Stock Availability** — Display available food items and their current stock quantities.

## Technology Stack

- **Language:** C#
- **Framework:** ASP.NET Core Razor Pages
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Frontend:** HTML, CSS, JavaScript
- **UI Framework:** Bootstrap 5
- **Development Environment:** Visual Studio
- **Database Management:** SQL Server Management Studio (SSMS)

## Prerequisites

Before running the application, make sure you have installed:

1. Visual Studio with the ASP.NET and web development workload.
2. The .NET SDK version required by the project.
3. Microsoft SQL Server.
4. SQL Server Management Studio (SSMS).


## Database Setup

The application uses a Microsoft SQL Server database named `KarenderyaDB`.

### 1. Restore the Database

If a database backup file (`KarenderyaDB.bak`) is included:

1. Open SQL Server Management Studio.
2. Connect to your SQL Server instance.
3. Right-click **Databases** and select **Restore Database**.
4. Select **Device** as the source and browse for `KarenderyaDB.bak`.
5. Confirm the database name is `KarenderyaDB`.
6. Complete the restoration and verify that the database tables are available.

### 2. Configure the Connection String

Open `appsettings.json` and update the connection string to match your local SQL Server instance.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=KarenderyaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER_NAME` with your actual SQL Server instance name.

**Note:** `DefaultConnection` is an example key. Use the connection string name referenced in your application's `Program.cs`. If the application uses SQL Server authentication, configure the appropriate credentials securely.

### 3. Verify Database Connectivity

Before running the application, make sure:

- SQL Server is running.
- The `KarenderyaDB` database has been restored successfully.
- The configured connection string is correct.
- The database user or Windows account has the necessary permissions.

## Installation and Running

1. Clone or download the project repository.
2. Open the solution (`.sln`) or project (`.csproj`) in Visual Studio.
3. Restore the required NuGet packages.
4. Configure the SQL Server connection string.
5. Restore the database backup, if provided.
6. Build the solution and resolve any dependency or configuration errors.
7. Run the application using Visual Studio or the appropriate `dotnet run` command.

The application should open in your browser using the launch URL configured by the project.

## Usage

### Administrator

An administrator can access the administrative features assigned to the Admin role, including the available food and inventory management functions.

### Cashier

A cashier can:

- View available food items and stock quantities.
- Create customer orders.
- Enter quantities for selected food items.
- Review the calculated order total.
- Receive a success confirmation after creating an order.
- View previous orders and inspect their transaction details.

The available functions depend on the features implemented in the current version.

## Authentication and Security

The application includes basic security measures:

- Password hashing for stored passwords.
- Account activation checks.
- Session-based authentication.
- Role-based access restrictions.
- Server-side checks to prevent unauthorized access to protected pages.
- Logout functionality that clears the user's session.

These measures provide a foundation for access control but do not guarantee complete application security. Additional security review and hardening are necessary before production deployment.

## Current Limitations

This project was developed to meet the initial requirements within a limited development timeframe. It focuses on the core functionality required for basic food inventory management and order processing.

- **Basic Security Implementation** — Authentication, password hashing, account activation, and page access restrictions are implemented. However, further security review and enhancement are needed before production use.
- **Limited Reporting** — A dedicated reporting module for sales summaries, transaction trends, and inventory reports has not yet been implemented.
- **Basic Cashier Management** — A dedicated module for managing cashier accounts and related administrative operations has not yet been implemented.
- **Menu Organization** — The current menu structure does not yet provide a dedicated drinks category or separate drinks management module.
- **User Interface** — The interface focuses on the required functionality. Further visual refinements and usability improvements would help make the application more polished and consistent.
- **Production Readiness** — Additional testing, security hardening, deployment configuration, logging, and operational monitoring would be needed before production use.

## Future Improvements

The following features and enhancements may be considered if the project is extended.

### 1. Cashier Management Module

- Create and manage cashier accounts.
- Activate or deactivate cashier accounts.
- Update cashier information and access permissions.
- Improve administrative oversight of cashier activities.

### 2. Reports Module

- Generate daily, weekly, and monthly sales reports.
- Display transaction counts and sales summaries.
- Identify best-selling food and beverage items.
- Generate inventory and stock movement reports.
- Support exporting reports for recordkeeping and analysis.

### 3. Separate Drinks Menu

- Introduce a dedicated category or module for drinks.
- Organize food and beverage items separately.
- Improve menu browsing and inventory management.
- Allow additional menu categories to be introduced in the future.

### 4. UI/UX Improvements

- Improve the overall visual design and consistency.
- Refine navigation, forms, tables, and modal dialogs.
- Improve responsiveness across desktop, tablet, and mobile devices.
- Provide clearer validation messages, loading indicators, and feedback for user actions.
- Improve accessibility and ease of use.

### 5. Security Enhancements

- Review authentication and authorization across all pages and request handlers.
- Strengthen session management and account security.
- Review protections against common web application vulnerabilities.
- Add audit logging for important administrative and transaction activities.
- Improve configuration and secret management.
- Review database access permissions and production deployment security.

### 6. Testing and Production Readiness

- Add unit and integration tests for critical workflows.
- Test order processing and stock deduction under concurrent requests.
- Improve exception handling and application logging.
- Review database backup and recovery procedures.
- Configure secure deployment, monitoring, and maintenance procedures.

## Development Notes

This application was developed as a functional project, with an emphasis on meeting the initial requirements within the available timeframe.

The current implementation demonstrates the core workflows for authentication, food inventory management, and order processing. The limitations and future improvements documented above identify areas that could be enhanced in a subsequent development phase.

The planned enhancements are not part of the current version unless explicitly implemented.

## Database Backup

A SQL Server database backup may be provided separately as `KarenderyaDB.bak` to support local restoration and testing.

Database backups should be reviewed before sharing to ensure they do not contain real passwords, personal information, or other sensitive data.

## Author
Jona Busante Aurellano
Developed as a Karenderya Management System project.
