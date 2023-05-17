Bank Aurora Finance


This is a web application built using ASP.NET Core and Razor Pages. It allows customers to view their accounts and transactions, and allows cashiers to manage customer accounts and transactions. It also includes an admin role that can manage users.



Features

Authentication and Authorization: This application uses ASP.NET Core Identity for authentication and authorization. Two types of users are seeded into the database: cashiers and admins.


Login cashier 

Username: richard.erdos.chalk@gmail.se
Password: Hejsan123#

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



Technologies Used:

ASP.NET Core 6.0

Razor Pages

Entity Framework Core (Database First)

ASP.NET Core Identity

AutoMapper

Bootstrap (for styling)

Web API

Console Application

JavaScript

Azure



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



Input Validation

This application handles input validation to ensure that all user input is valid and safe. This includes server-side validation and client-side validation using JavaScript.



Search Functionality

This application includes search functionality that allows users to search for customers and accounts. The search functionality is implemented using LINQ queries.



Javascript Functionality

This application uses JavaScript to implement a show more function that allows users to load more transactions on the customer's page.

It also uses JavaScript to implement an autofill function in the customer form. When a user enters a customer's country, the form is automatically populated with more details.



Pagination

This application uses pagination to display the customer and accounts lists. This improves performance and makes it easier for users to navigate the lists.



Back Button

This application includes a back button that allows users to go back to the previous page. The back button is implemented as a partial and is included on most pages.



Deployment

This application is published to Azure. Users can access the application from anywhere with an internet connection.
Link to project in Azure: https://aurorafinance.azurewebsites.net


















