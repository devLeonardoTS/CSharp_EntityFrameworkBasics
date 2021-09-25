# Learning the basics of Entity Framework.
In this project I follow the steps of [Les Jackson](https://www.youtube.com/c/binarythistle) with his [Task Master](https://dotnetplaybook.com/introduction-to-entity-framework/) project, where he demonstrated how to develop a simple ToDo/Tasks List using the .Net Windows Forms platform along with the Entity Framework to help performing CRUD Operations against a SQL Database.

The diferences though, are subtle. As for the .Net platform I'm using .Net 5 while the original project used .Net Framework 4.5.2. Also instead of SQL Server Express, I integrated the Entity Framework's DbContext with a SQLite Database. There are also some fiddling around with user input validations which are far from ideal but are there and working fine.