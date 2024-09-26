# Hospital Management System

This project is a comprehensive **Hospital Management System** built using .NET Core. It provides REST APIs for managing various aspects of a hospital, including patient records, doctor details, consultations, prescriptions, treatments, and more.

## Features

- **User Management**: 
  - Supports authentication and authorization with ASP.NET Core Identity.
  - Role-based access control for different types of users (e.g., Admin, Doctor, Nurse, Patient).
  
- **Entities Managed**:
  - **Patients**: Track patient details, including personal information, medical history, and appointments.
  - **Doctors**: Manage doctor profiles, including specialization, schedules, and consultation fees.
  - **Consultations & Prescriptions**: Manage patient consultations, medical diagnoses, and prescriptions.
  - **Departments & Specializations**: Define hospital departments and doctor specializations.
  - **Medicines & Vaccines**: Track medicine inventory and manage vaccine appointments.
  - **Test Results & Treatments**: Record test results and ongoing treatments for patients.
  - **Building & Floors**: Track hospital buildings and floor management.
  - **Expenses**: Track and manage hospital-related expenses.

## Technologies Used

- **.NET Core 6/7/8**: Framework for building the APIs.
- **Entity Framework Core**: ORM for managing the database with Code First approach.
- **SQL Server**: Database for persisting hospital data.
- **Swagger/OpenAPI**: For API documentation and testing.
- **Identity Framework**: For authentication and role-based authorization.

