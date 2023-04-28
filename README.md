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
GET /businesses - gets all businesses
POST /businesses - adds new business 
GET /businesses/{id} - gets business by id
PUT /businesses/{id} - edits business by id
DELETE /businesses/{id} - deletes business by id
GET /businesses/{id}/certificates - gets certificates by business id

### Certificates 
GET /certificates - gets all certs
POST /certificates - add new cert
GET /certificates/{id} - get cert by id
PUT /certificates/{id} - edit cert by id
DELETE /certificates/{id} - delete cert by id
PUT /certificates/{id}/freeze - freeze a cert
PUT /certificates/{id}/unfreeze - unfreeze a cert

### Courses
GET /courses - gets all courses
POST /courses - adds new course
GET /courses/{id} - get course by id
PUT /courses/{id} - edit a course
GET /courses/businesses/{id} - get courses by business 
DELETE /courses/{id} - delete a course

### Employees 
GET /employees - gets all employees
POST /employees - add new employee
GET /employees/{id} - get employee by id
PUT /employees/{id} - edit employee
DELETE /employees/{id} - delete employee
POST /employees/logout - logout employee

### End Users
GET /endusers - get all end users
POST /endusers - add new end user (register)
GET /endusers/{id} - get enduser by id
PUT /endusers/{id} - edit end user
DELETE /endusers/{id} - delete end user by id
GET /endusers/{id}/certificates - get end users certs
PUT /endusers/certificates/unassign/{id} - unassign a cert from an end user
POST /endusers/logout - logout end user 

### Flagged Issues
GET /flaggedusses - get all flagged issues
POST /flaggedusses - post flagged issue
GET /flaggedusses/{id} - get flagged issue by id
PUT /flaggedusses/{id} - mark flagged issue as read
DELETE /flaggedusses/{id} - delete flagged issue by id
GET /flaggedusses/businesses/{id} - get flagged issues per business

### Login 
POST /endusers/login - login end user
POST /employees/login - login employee
POST /admins/login - login admin

All end points return data in JSON format. 

To use this API, you can send HTTP requests to these endpoints using Postman or Swagger in the browser. 
