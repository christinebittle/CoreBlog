# ASP.NET Core Blog Application
This example connects our server to a MySQL Database with MySql.Data.MySqlClient.

- Models/BlogDbContext.cs
    - A class which represents the connection to the database. Be mindful of the connection string fields!
- Controllers/AuthorAPIController.cs
    - An API Controller which allows us to access information about Authors
- Program.cs
    - Configuration of the application

## How to Install:
1. Download [MAMP](https://www.mamp.info/en/downloads/) or similar MySQL environment
2. [Download "authors.sql", "articles.sql", "comments.sql", "tags.sql" from the datagenerator](http://sandbox.bittsdevelopment.com/humber/datagenerator/)
3. Create a blog database 
    - PhpyMyAdmin -> new database -> blog
4. : Import Tables [VIDEO GUIDE](https://youtu.be/wWMcIza-k4s)
  - PhpMyAdmin -> Import -> Upload authors.sql
  - PhpMyAdmin -> Import -> Upload articles.sql
  - PhpMyAdmin -> Import -> Upload comments.sql
  - PhpMyAdmin -> Import -> Upload tags.sql
5. Access Connection String properties for your blog DB and change User, Pass, Port, Database, Server in "/Models/BlogDbContext.cs"
6. Make sure "MySQL.Data" is installed in your Visual Studio environment
    - If not installed, go to "Tools" > "Nuget Package Manager" > "Manage Nuget Packages for Solution" > "Browse" > type "MySQL.Data" > "Install"
7. Run the project debugging mode (F5) **while** the database environment is running
8. : Test to see if the ListAuthors WebAPI method returns information about authors.
    - GET api/AuthorData/ListAuthors

Test Your Understanding! (Exercises)
- Create a TagAPIController.cs
- Create a method ListTagNames which outputs a List<string> of the tag name and Tag Color
- Create an ArticleAPICotroller.cs
- Create a method ListArticles which outputs a List<string> of the Article Title and Article Date
