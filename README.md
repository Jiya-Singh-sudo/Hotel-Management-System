# Hotel Management System

A robust, web-based Hotel Management System built with **ASP.NET Core (MVC)** and **Entity Framework Core**, designed to streamline hotel operations including bookings, customer management, and room tracking.

## 🚀 Features

* **User Authentication & Authorization**: Secure account registration and login system with cookie-based authentication, supporting both "Admin" and "Customer" roles.
* **Booking Management**: Intuitive booking creation, editing, and tracking system.
* **Admin Dashboard**: Dedicated access for administrators to manage operations efficiently.
* **Automated Admin Seeding**: Automatically creates a default system administrator account upon application startup for immediate access.
* **Secure Password Handling**: Implements `BCrypt.Net` for industry-standard password hashing.

## 🛠 Tech Stack

* **Framework**: ASP.NET Core 9.0 (MVC/Razor Pages)
* **Database**: PostgreSQL
* **ORM**: Entity Framework Core 9.0
* **Security**: `BCrypt.Net-Next` for password hashing

## 📋 Prerequisites

* [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
* [PostgreSQL Database](https://www.postgresql.org/)

## ⚙️ Setup and Installation

1. **Clone the Repository**
```bash
git clone <your-repository-url>
cd HotelManagementSystem

```


2. **Configure Database Connection**
Update your `appsettings.json` file with your PostgreSQL connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=YourDbName;Username=YourUsername;Password=YourPassword"
}

```


3. **Run Migrations**
Open your terminal in the `HotelManagementSystem` directory and run:
```bash
dotnet ef database update

```


4. **Launch the Application**
```bash
dotnet run

```


*The application will automatically seed a default admin user on the first run with the email `admin@staywise.com` and password `Admin@123`.*

## 📁 Key Project Structure

* **/Controllers**: Handles business logic (Authentication, Bookings, Customers, etc.).
* **/Models**: Contains data entities (Customer, Booking, Room, etc.) and view models.
* **/Data**: Contains the `ApplicationDbContext` for database interactions.
* **/Views**: Contains the Razor/HTML templates for the user interface.
