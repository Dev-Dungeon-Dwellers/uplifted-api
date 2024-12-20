# UpliftedAPI
## Project Description
This is the RESTful API for the Uplifted application. This project supports data access and manipulation for the Uplifted database.
## Development Environment Setup
### Prerequisits
Make sure you have the following installed:
* Visual Studio 2022
* All packages and installations required for creating and maintaining a .NET Web API application
* MySQL
* SQL Server Express
* Microsoft SQL Server Management Studio or other desired database management application
* Git

### Setup Steps
1. Fork the uplifted-api repository
2. Clone your forked repository onto your system:
`git clone https://github.com/{YOU}/uplifted-api.git`
3. Open the project solution in Visual Studio 2022
4. Click on Tools>NuGet Package Manager>Package Manager Console
5. In the presented console, install the following packages with the commands listed below:
  ```
  dotnet add package Microsoft.EntityFrameworkCore
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer
  dotnet add package Microsoft.EntityFrameworkCore.Tools
  dotnet add package Microsoft.EntityFrameworkCore.Design
  ```
6. Go to Microsoft SQL Server Management Studio and conect to your MySQL LocalDB instance.
* For example, my connection string is `Data Source=(localdb)\\Local;Initial Catalog=uplifteddb;Integrated Security=True;Encrypt=True`
7. Run a database migration by executing the following commands in the NuGet Package Manager Console
```
ADD-MIGRATION MyFirstMigration
UPDATE-DATABASE
```
* This will apply the migration to your database and produce all of the datatables you need on a local instance.

All of this should allow you to run the application and test the API endpoints via SwaggerAPI

## How to Contribute
1. Create a branch with the following format: `issuenumber-shortdescription`
2. make changes and commit
3. push up to your forked repo
4. create pull request on main repo
