# UFS Qwaqwa Campus Bank - Web & Mobile Banking Application

## Table of Contents
- [Project Overview](#project-overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Usage](#usage)
- [Future Plans](#future-plans)

## Project Overview

This project is a web and mobile banking application for the University of the Free State, QwaQwa Campus, designed to streamline financial services for students and staff. The system supports basic banking functionalities such as account management, fund transfers, and transaction histories. The project is built using a client-server architecture for secure and efficient operations.

## Features

- **Admin Panel:**
  - Manage user accounts (students, staff)
  - View user activity and account statuses
  - Manage consultants and authorize their actions
  - Generate reports on transactions and user activities

- **Consultant:**
  - Assist customers with transactions (deposits, withdrawals)
  - View and update user account information
  - Generate reports on client activity
  - View customer feedback and ratings

- **Financial Advisor:**
  - View user accounts
  - Provide financial advice based on account data

- **Customers (Students/Staff):**
  - Register and log in to their accounts
  - Perform transactions like transferring funds between accounts
  - View personal and account information
  - Receive real-time notifications for transactions and updates

## Technologies Used

- **Frontend (Web):**
  - .NET 5.0 MVC
  - Bootstrap 5
  - MS Visual Studio 2022

- **Mobile (Android):**
  - Android Studio (API Level 24 and above)

- **Backend:**
  - Microsoft SQL Server 2019
  - Firebase for authentication and secure storage

- **Communication:**
  - SSH for secure communication between the web application and the database server

## Usage

- **Admin:**
  - Navigate to `/admin/login` to access the admin dashboard.
  
- **Consultant:**
  - Log in via `/consultant/login` to manage users and assist with transactions.
  
- **Customer (Student/Staff):**
  - Register or log in to view your account and manage your transactions.

## Future Plans

I plan to enhance this application by integrating a **recommendation system**. This system will leverage **Data Science techniques** to analyze transaction data and provide personalized financial advice to users. Potential use cases include:
- Predictive analysis for spending patterns
- Custom-tailored recommendations for savings and investments
- Personalized loan or credit suggestions based on user behavior
