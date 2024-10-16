# Business Card Manager Backend

This repository contains the backend service for the **Business Card Manager** application. It is built using **.NET 8** with an Onion Architecture and is designed to handle the backend logic,
including CRUD operations, database management, and business card export functionality.

## Features

- **CRUD Operations**: Manage business cards including creation, retrieval, and deletion.
- **Export Functionality**: Export business card details in either **CSV** or **XML** format.
- **Photo Field**: Photos can be added or left blank, and stored in base64 format.

## Technologies Used

- **.NET 8**: The core backend framework.
- **Onion Architecture**: Separation of concerns between API, Services, and Repositories layers.
- **Entity Framework Core**: For database interactions and migrations.
- **SQL Server**: Database system.
- **Dependency Injection**: For service and repository management.

## Project Structure

The backend project follows the Onion Architecture, divided into the following layers:

- **API Project**: Handles HTTP requests via controllers.
- **Domain Project**: Contains core models and interfaces.
- **Services Project**: Implements the business logic of the application.
- **Repositories Project**: Handles data access and database management.

## Setup Instructions

1. **Clone the repository**:
   ```bash
   git clone https://github.com/qarqaz/BusinessCardManagerBackend.git

2. **Navigate to the backend project**:
   ```bash
   cd BusinessCardManagerBackend
   
3. **Set up the database**:
   
To quickly set up the database for this project, a backup file is provided.

1. Download the `BusinessCardDb.bak` file from the [db-backups folder](https://github.com/qarqaz/BusinessCardManagerBackend/blob/StableReleaseV1/BusinessCardDb.bak)
2. Restore the database in **SQL Server** using the following steps:
   - Open SQL Server Management Studio (SSMS).
   - Right-click on `Databases`, then select `Restore Database`.
   - Choose `Device`, click the `...` button, and locate the `BusinessCardDb.bak` file.
   - Click `OK` to restore the database.
   
Make sure to update the connection string in `appsettings.json` to match your local SQL Server setup.
   
4. **Apply migrations**:
   ```bash
    dotnet ef database update
   
5. **Run the backend service**:
   ```bash
    dotnet run

## API Endpoints

- **GET**: /api/businesscard: Get all business cards.
- **GET**: /api/businesscard/{id}: Get a specific business card by ID.
- **POST**: /api/businesscard/create: Create a new business card.
- **PUT**: /api/businesscard/{id}: Update an existing business card.
- **DELETE**: /api/businesscard/{id}: Delete a business card.
- **GET**: /api/businesscard/export/{id}/{format}: Export a business card in CSV (1) or XML (2) format.

## Unit Testing

   ```bash
    dotnet test
