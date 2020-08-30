# a3-s3740446-s3734938
Web Development Technologies Flexible Semester 2020 Assignment 3

Ryan Cassidy - s3740446

Vineet Bugtani - s3734938

Equal contribution by both assignment partners

Project runs in Visual Studio

Solution File:
\AngularAdminPage\AngularAdminPage.sln 

This project is a dual project running under a single solution, in order to run both projects simultaneously open the properties of the solution and set it to multiple startup projects with both projects set to "Start".

Angular page may also be missing packages as the node-module folder is not included here. Run a npm install or update in the ClientApp folder to resolve this.

Administration page created in Angular

Should the admin application be broken. Please make sure the ports in the APIInterceptor service are set to whatever port number Visual Studio assigns to the BankingApplication project on runtime.

The service can be found under ClientApp\src\app\services\ApiInterceptor.service.ts. Change the port number in the apiURL variable to the appropriate one.

The angular page provides control to an administrator to view all customers within the banking application and perform actions such as editing their customer profile, deleting them, blocking their accounts and blocking bills.

The app also graphs transaction information into pie and bar graphs.

It is made secure through using a route guard service that will redirect anyone not logged in back to the home page.

The admin page makes API calls to the banking application which is handled under AdminController. AdminController uses a repository pattern utilized in the previous version of our assignment to provide data and control to the admin page via API requests. 

Other features implemented in this project include login locking, status code error pages, user timeout and bill locking. All of which are implemented on the net core application side.

References:
Custom error page setup 
https://gooroo.io/GoorooTHINK/Article/17086/Creating-Custom-Error-Pages-in-ASPNET-core-10/32407
