# ASP.NET Core Blog Application
This example connects our server to a MySQL Database with MySql.Data.MySqlClient.

- Models/BlogDbContext.cs
    - A class which represents the connection to the database. Be mindful of the connection string fields!
- Controllers/AuthorAPIController.cs
    - An API Controller which allows us to access information about Authors
- Program.cs
    - Configuration of the application


Test Your Understanding!
- Create a TagAPIController.cs
- Create a method ListTagNames which outputs a List<string> of the tag name and Tag Color
- Create an ArticleAPICotroller.cs
- Create a method ListArticles which outputs a List<string> of the Article Title and Article Date
