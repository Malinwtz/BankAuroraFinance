Bank Aurora Finance


This is a web application built using ASP.NET Core and Razor Pages. It allows customers to view their accounts and transactions, and allows cashiers to manage customer accounts and transactions. It also includes an admin role that can manage users.


Technologies Used

ASP.NET Core
Razor Pages
Entity Framework Core (Database First)
ASP.NET Core Identity
AutoMapper
Bootstrap (for styling)
Web API
Console Application


Features

Authentication and Authorization: This application uses ASP.NET Core Identity for authentication and authorization. Two types of users are seeded into the database: cashiers and admins.

Cashiers can perform the following tasks:

View a list of customers
View a list of transactions
Create new customers
Make transactions for customers


Admins can perform the following tasks:

View a list of all users
Create new users
Edit user roles
Delete users


Database

This application uses Entity Framework Core with the Database First approach to interact with a SQL Server database. The database contains three main tables: customers, accounts, and transactions. A separate library has been created to handle interactions with the database.


AutoMapper

This application uses AutoMapper to map between the data models and view models.


Styling

This application uses Bootstrap and a styling template that has been customized to match the bank.


Web API

This application includes a Web API that allows other applications to access the customer and transaction data.


Console Application

This application includes a console application that checks for suspicious transfers. The console application uses the Web API to retrieve the necessary data.

