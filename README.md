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
5. Access Connection String properties for your blog DB and change User, Pass, Port, Database, Server in "/Models/BlogDbContext.cs".
6. Make sure "MySQL.Data" is installed in your project
    - If not installed, go to "Tools" > "Nuget Package Manager" > "Manage Nuget Packages for Solution" > "Browse" > type "MySQL.Data" > "Install"
7. Run the project debugging mode (F5) **while** the database environment is running
8. : Test to see if the ListAuthorNames API responds with information about authors.
    - GET api/Author/ListAuthorNames
   
## Common Errors
- 'MySql.Data.MySqlClient.MySqlException: 'Unable to connect to any of the specified MySQL hosts, Inner Exception: No connection could be made because the target machine actively refused it'
    - Check that your database is running AND the connection string settings (server, port, database) are accurate!
- 'MySql.Data.MySqlClient.MySqlException: 'Authentication to host .. failed with message: Access denied for user ..'
    - Check that the user name and password fields are accurate!
- MAMP doesn't work!
    - [Try XAMMP](https://www.apachefriends.org/), or any MySQL environment you have access to with a connection string.
   
## Exercises
Test Your Understanding by accomplishing these tasks!
### Exercise 3
- Create Article.cs model
- Create "Find Article" functionality in Article Controller
- Create MVC Controller ArticlePageController
- Create Views List and Show to List and Show Articles
### Exercise 4
- Create Tag.cs model
- Create "Find Tag" functionality in Tag Controller
- Create MVC Controller TagPageController
- Create Views List and Show to List and Show Tags
