# HealthCare

## Description
This application is a patient management system that allows patients to manage their appointments, records, and find nearby pharmacies and hospitals using the Google Maps API.

## Features
- View and create appointments
- View and create medical records
- Find nearby pharmacies and hospitals
- Authentication and authorization for patients

## Technologies Used
- ASP.NET Core
- Google Maps API
- Entity Framework Core
- Microsoft SQL Server

## Prerequisites
Before running the application, ensure that you have the following installed:
- .NET Core SDK [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
- SQL Server [https://www.microsoft.com/sql-server](https://www.microsoft.com/sql-server)

## Setup
1. Clone the repository or download the source code.
2. Open the project in your preferred development environment.
3. Configure the database connection string in the `appsettings.json` file.
4. Obtain a Google Maps API key from the Google Cloud Platform Console and add it to the `appsettings.json` file.
5. Run the database migration to create the required tables and schema:
    -dotnet ef database update 
6. Build the project:
    -dotnet build
7. Run the application:
    -dotnet run
8. Open a web browser and go to `http://localhost:5000` to access the application.

## Usage
- Register a new patient account or log in as an existing patient.
- View and create appointments using the provided endpoints.
- View and create medical records using the provided endpoints.
- Use the `/api/patient/pharmacies` endpoint to find nearby pharmacies.
- Use the `/api/patient/hospitals` endpoint to find nearby hospitals.

## License
This project is licensed under the [MIT License](LICENSE).

## Contributing
Contributions are welcome! If you find any issues or have suggestions for improvements, feel free to create a pull request or submit an issue.

