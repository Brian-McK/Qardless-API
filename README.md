# Qardless API 

README file for Qardless-API, a RESTful API built using ASP .NET Core and using Enitity Framework, connected to a Microsoft SQL Server Database. 

## Installation 

To run this API, you will need: 
* .NET Core 6 or above 
* Microsoft SQL Server  
* Recommended IDE: Visual Studio 2023 

### Create a Local SQL Server DB on your machine

1. Download and install Microsoft SQL Server Management Studio.
2. Open up command prompt.
3. Run command 'sqllocaldb create Local'
4. Open Microsoft SQL Server Management Studio. 
5. Server name: '(localdb)\Local'
6. Auth: 'Windows Auth' 
7. In command prompt, go to project directory, specifically '/Qardless.API'
8. If no migrations: run command 'dotnet ef migrations add UpdateMigration'
9. If migrations: run command 'dotnet ef database update'
10. Database should be viewable in Microsoft SQL Server Management Studio. 

* Modify appsettings.json' to point to your own SQL Server Database. 

## Usage 

The API provides the following endpoints: 

### Admins 
GET /admins - gets all admins <br />
POST /admins - adds a new admin <br />
GET /admins/{id} - gets admin by id <br />
PUT /admins/{id} - edits admin by id <br />
DELETE /admins/{id} - deletes an admin by id <br />
POST /admins/logout - logouts an admin <br />

### Businesses 
GET /businesses - gets all businesses <br />
POST /businesses - adds new business <br />
GET /businesses/{id} - gets business by id <br />
PUT /businesses/{id} - edits business by id <br />
DELETE /businesses/{id} - deletes business by id <br />
GET /businesses/{id}/certificates - gets certificates by business id <br />

### Certificates 
GET /certificates - gets all certs <br />
POST /certificates - add new cert <br />
GET /certificates/{id} - get cert by id <br />
PUT /certificates/{id} - edit cert by id <br /> 
DELETE /certificates/{id} - delete cert by id <br />
PUT /certificates/{id}/freeze - freeze a cert <br />
PUT /certificates/{id}/unfreeze - unfreeze a cert <br />

### Courses
GET /courses - gets all courses <br /> 
POST /courses - adds new course <br /> 
GET /courses/{id} - get course by id <br /> 
PUT /courses/{id} - edit a course <br /> 
GET /courses/businesses/{id} - get courses by business  <br />
DELETE /courses/{id} - delete a course <br /> 

### Employees 
GET /employees - gets all employees <br />
POST /employees - add new employee <br /> 
GET /employees/{id} - get employee by id <br /> 
PUT /employees/{id} - edit employee <br /> 
DELETE /employees/{id} - delete employee <br />
POST /employees/logout - logout employee <br /> 

### End Users
GET /endusers - get all end users <br />
POST /endusers - add new end user (register) <br /> 
GET /endusers/{id} - get enduser by id <br /> 
PUT /endusers/{id} - edit end user <br /> 
DELETE /endusers/{id} - delete end user by id <br /> 
GET /endusers/{id}/certificates - get end users certs <br /> 
PUT /endusers/certificates/unassign/{id} - unassign a cert from an end user <br /> 
POST /endusers/logout - logout end user  <br /> 

### Flagged Issues
GET /flaggedusses - get all flagged issues <br /> 
POST /flaggedusses - post flagged issue <br />
GET /flaggedusses/{id} - get flagged issue by id <br /> 
PUT /flaggedusses/{id} - mark flagged issue as read <br />
DELETE /flaggedusses/{id} - delete flagged issue by id <br />
GET /flaggedusses/businesses/{id} - get flagged issues per business <br /> 

### Login 
POST /endusers/login - login end user <br />
POST /employees/login - login employee <br />
POST /admins/login - login admin <br />

All end points return data in JSON format. 

To use this API, you can send HTTP requests to these endpoints using Postman or Swagger in the browser. 
