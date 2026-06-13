## Interview Task Major question **_Important_**

How does data flow from the Angular UI to the backend?
**_Answer_**
On appliation start we send a Http Get request to backend to fetch the stored tasks on db
then we store it on our state of tasks on the frontend or angular UI

How does Dependency Injection work in your backend?
**_Answer_**

On building or on compling the application. the builder or complier pass the classes of the called Dependency on every build why? to ensure that our code is loosly coupled and the reuse of functions and methods is easy and also make the process of changing mutiple used Dependency throw the application easer

How do you handle validation and errors?
**_Answer_**
on frontend we check the input values and ensure that they are on our standards
on backend we spacify the types and db rules and if any sent content is not correct we pass error message to frontend with type of error

Which part was hardest for you and why?
**_Answer_**
the backend was a bit challenging cause it was new stack for me but i did a good research to understand concepts
the db part was chanllenging cause i had to use raw sql quires but it was easier that what i expected

If this app had 10,000 tasks, what problems might occur and what do you suggest to fix?
**_Answer_**
there is alot of technices we can apply like Virtual Scrolling: which is distrubuting data into different sets and show only one set of data at once then on Scrolling we fetch more data from the server insted of fetching all data at once
we also can reduce backend sent data by only sending importnat or requeted data only
we also can sort data by adding filters and we apply defult filters so we fetch requried data only
we also can cash queries on frontend so we dont performe alot of request at the same time

# Task Manager App

Simple Crud app made for learning more about angular and asp.net api

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org) and npm
- Angular CLI: `npm install -g @angular/cli`

## Backend Setup (API)

# Install the only required package: SQLite

dotnet add package Microsoft.Data.Sqlite

The API runs at `https://localhost:5266`

## Frontend Setup (Client)

# Install dependencies

npm install

# Run the app

ng serve

The Angular app runs at `http://localhost:4200`.

## Using the App

1. Start the **API** first (`dotnet run` in `/api`).
2. Start the **Angular app** (`ng serve` in `/client`).
3. Open `http://localhost:4200` in your browser.
   The Angular app talks to the API to create, read, update, and delete tasks.
