# Talent Management Application

This is a demo practice ASP.NET Core MVC Application designed for HR Administrators to enrol employees into various different courses or Trainings they 
may need in order to upskill themselves or if they want to learn anything new that may be beneficial for them

This application contains: Course Management Section

                        Add Courses or Trainings to the system
                        Upload Course content files
                        View Course details
                        Update Course Details
                        Download Courses to excel
                        Remove courses
                        
This application contains: Employee Management Section

                        Add Employees to the system
                        Upload Employee Files
                        View Employee details
                        Update Employee Details
                        Download Employees to excel
                        Remove Employees
                        
This application contains: User Management Section

                        Add Users to the system
                        View Users details
                        Update Users Details
                        Remove Users
                        Assign Roles to Users
                        

Uses Cookie Authentication and Authorization functionality and MD5 Password hashing. 
The application contains 3 roles for users: Viewer, HR Administrator and Super Administrator. You can add new users and assign any roles to these users.

Use The following Email and Password to Authenticate into the application:
                                          
                                          Email: JamieLannister@GameOfThrones.com (Super Administrator)
                                          Password: P@ssword123
                                          
                                          Email: JohnSnow@GameOfThrones.com (Viewer)
                                          Password: P@ssword123
                                          
                                          Email: CerseiLannister@GameOfThrones.com (HR Administrator)
                                          Password: P@ssword123

Connects to the database using Entity Framework Core with database first approach.
Download the application database Here: [Application Database.zip](https://github.com/Sax-Anon90/TalentManagementApplication/files/8716743/Application.Database.zip)

Uses Non-generic repository and Unit of work pattern with Dependancy Injection

View Models and AutoMapper

Custom Admin Bootstrap Layout found here: https://www.wrappixel.com/templates/ample-admin-lite/

Logging using SeriLog
