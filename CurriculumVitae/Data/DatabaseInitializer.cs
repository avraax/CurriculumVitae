using Microsoft.Data.Sqlite;
using CurriculumVitae.Models;

namespace CurriculumVitae.Data;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await CreateTablesIfNotExistAsync(connection);
        await SeedDataIfEmptyAsync(connection);
    }

    private async Task CreateTablesIfNotExistAsync(SqliteConnection connection)
    {
        var createEducationTable = @"
            CREATE TABLE IF NOT EXISTS Education (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Institution TEXT NOT NULL,
                Degree TEXT NOT NULL,
                FieldOfStudy TEXT,
                StartDate TEXT NOT NULL,
                EndDate TEXT,
                Grade TEXT,
                Description TEXT
            )";

        var createSkillTable = @"
            CREATE TABLE IF NOT EXISTS Skill (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                ProficiencyLevel INTEGER NOT NULL,
                Category INTEGER NOT NULL,
                YearsOfExperience INTEGER,
                Description TEXT
            )";

        var createProjectTable = @"
            CREATE TABLE IF NOT EXISTS Project (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT NOT NULL,
                Technologies TEXT,
                StartDate TEXT NOT NULL,
                EndDate TEXT,
                ProjectUrl TEXT,
                SourceCodeUrl TEXT,
                IsFeatured INTEGER NOT NULL DEFAULT 0
            )";

        var createCompanyTable = @"
            CREATE TABLE IF NOT EXISTS Company (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT,
                Industry TEXT,
                Location TEXT,
                Website TEXT,
                EmployeeCount INTEGER,
                FoundedDate TEXT,
                Notes TEXT
            )";

        // Execute table creation commands
        using var command = connection.CreateCommand();
        
        command.CommandText = createEducationTable;
        await command.ExecuteNonQueryAsync();
        
        command.CommandText = createSkillTable;
        await command.ExecuteNonQueryAsync();
        
        command.CommandText = createProjectTable;
        await command.ExecuteNonQueryAsync();
        
        command.CommandText = createCompanyTable;
        await command.ExecuteNonQueryAsync();
    }

    private async Task SeedDataIfEmptyAsync(SqliteConnection connection)
    {
        // Check and seed education
        using var educationCheckCommand = connection.CreateCommand();
        educationCheckCommand.CommandText = "SELECT COUNT(*) FROM Education";
        var educationCount = Convert.ToInt32(await educationCheckCommand.ExecuteScalarAsync());

        if (educationCount == 0)
        {
            await SeedEducationAsync(connection);
        }

        // Check and seed skills
        using var skillCheckCommand = connection.CreateCommand();
        skillCheckCommand.CommandText = "SELECT COUNT(*) FROM Skill";
        var skillCount = Convert.ToInt32(await skillCheckCommand.ExecuteScalarAsync());

        if (skillCount == 0)
        {
            await SeedSkillsAsync(connection);
        }

        // Check and seed projects
        using var projectCheckCommand = connection.CreateCommand();
        projectCheckCommand.CommandText = "SELECT COUNT(*) FROM Project";
        var projectCount = Convert.ToInt32(await projectCheckCommand.ExecuteScalarAsync());

        if (projectCount == 0)
        {
            await SeedProjectsAsync(connection);
        }

        // Check and seed companies
        using var companyCheckCommand = connection.CreateCommand();
        companyCheckCommand.CommandText = "SELECT COUNT(*) FROM Company";
        var companyCount = Convert.ToInt32(await companyCheckCommand.ExecuteScalarAsync());

        if (companyCount == 0)
        {
            await SeedCompaniesAsync(connection);
        }
    }

    private async Task SeedEducationAsync(SqliteConnection connection)
    {
        var educations = new[]
        {
            new { Institution = "University of Technology", Degree = "Bachelor of Science", FieldOfStudy = "Computer Science", StartDate = new DateTime(2012, 9, 1), EndDate = new DateTime(2016, 5, 15), Grade = "3.8 GPA", Description = "Graduated Magna Cum Laude. Relevant coursework: Data Structures, Algorithms, Software Engineering, Database Systems." },
            new { Institution = "Microsoft Learn", Degree = "Azure Developer Associate", FieldOfStudy = "Cloud Computing", StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 3, 15), Grade = "Certified", Description = "Earned AZ-204 certification demonstrating expertise in developing cloud solutions on Microsoft Azure platform." },
            new { Institution = "Stanford University", Degree = "Master of Science", FieldOfStudy = "Software Engineering", StartDate = new DateTime(2016, 9, 1), EndDate = new DateTime(2018, 6, 15), Grade = "3.9 GPA", Description = "Advanced coursework in distributed systems, machine learning, and software architecture." }
        };

        foreach (var edu in educations)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Education (Institution, Degree, FieldOfStudy, StartDate, EndDate, Grade, Description) 
                VALUES (@Institution, @Degree, @FieldOfStudy, @StartDate, @EndDate, @Grade, @Description)";
            
            command.Parameters.AddWithValue("@Institution", edu.Institution);
            command.Parameters.AddWithValue("@Degree", edu.Degree);
            command.Parameters.AddWithValue("@FieldOfStudy", edu.FieldOfStudy);
            command.Parameters.AddWithValue("@StartDate", edu.StartDate);
            command.Parameters.AddWithValue("@EndDate", edu.EndDate);
            command.Parameters.AddWithValue("@Grade", edu.Grade);
            command.Parameters.AddWithValue("@Description", edu.Description);
            
            await command.ExecuteNonQueryAsync();
        }
    }

    private async Task SeedSkillsAsync(SqliteConnection connection)
    {
        var skills = new[]
        {
            new { Name = "C#", ProficiencyLevel = 9, Category = (int)SkillCategory.Technical, YearsOfExperience = 8, Description = "Expert level proficiency in C# and .NET ecosystem" },
            new { Name = "ASP.NET Core", ProficiencyLevel = 9, Category = (int)SkillCategory.Technical, YearsOfExperience = 6, Description = "Extensive experience building web APIs and applications" },
            new { Name = "React", ProficiencyLevel = 8, Category = (int)SkillCategory.Technical, YearsOfExperience = 5, Description = "Proficient in React with TypeScript for frontend development" },
            new { Name = "SQL Server", ProficiencyLevel = 8, Category = (int)SkillCategory.Technical, YearsOfExperience = 7, Description = "Advanced T-SQL, database design, and performance optimization" },
            new { Name = "Azure", ProficiencyLevel = 7, Category = (int)SkillCategory.Technical, YearsOfExperience = 4, Description = "Cloud architecture and deployment on Microsoft Azure" },
            new { Name = "Docker", ProficiencyLevel = 7, Category = (int)SkillCategory.Technical, YearsOfExperience = 3, Description = "Containerization and orchestration with Docker and Kubernetes" },
            new { Name = "GraphQL", ProficiencyLevel = 6, Category = (int)SkillCategory.Technical, YearsOfExperience = 2, Description = "API design and implementation with GraphQL" },
            new { Name = "Team Leadership", ProficiencyLevel = 8, Category = (int)SkillCategory.Soft, YearsOfExperience = 5, Description = "Leading development teams and mentoring junior developers" },
            new { Name = "Project Management", ProficiencyLevel = 7, Category = (int)SkillCategory.Soft, YearsOfExperience = 4, Description = "Agile methodologies and project coordination" },
            new { Name = "English", ProficiencyLevel = 10, Category = (int)SkillCategory.Language, YearsOfExperience = 25, Description = "Native speaker" },
            new { Name = "Python", ProficiencyLevel = 7, Category = (int)SkillCategory.Technical, YearsOfExperience = 3, Description = "Data analysis and machine learning with Python" },
            new { Name = "TypeScript", ProficiencyLevel = 8, Category = (int)SkillCategory.Technical, YearsOfExperience = 4, Description = "Type-safe JavaScript development" }
        };

        foreach (var skill in skills)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Skill (Name, ProficiencyLevel, Category, YearsOfExperience, Description) 
                VALUES (@Name, @ProficiencyLevel, @Category, @YearsOfExperience, @Description)";
            
            command.Parameters.AddWithValue("@Name", skill.Name);
            command.Parameters.AddWithValue("@ProficiencyLevel", skill.ProficiencyLevel);
            command.Parameters.AddWithValue("@Category", skill.Category);
            command.Parameters.AddWithValue("@YearsOfExperience", skill.YearsOfExperience);
            command.Parameters.AddWithValue("@Description", skill.Description);
            
            await command.ExecuteNonQueryAsync();
        }
    }

    private async Task SeedProjectsAsync(SqliteConnection connection)
    {
        var projects = new[]
        {
            new { Name = "E-Commerce Platform", Description = "Full-stack e-commerce solution with microservices architecture, supporting 10,000+ concurrent users. Implemented using .NET 6, React, Redis, and PostgreSQL.", Technologies = "ASP.NET Core, React, TypeScript, Redis, PostgreSQL, Docker, Azure", StartDate = new DateTime(2022, 1, 1), EndDate = (DateTime?)new DateTime(2022, 8, 15), ProjectUrl = (string?)"https://demo-ecommerce.example.com", SourceCodeUrl = "https://github.com/johndoe/ecommerce-platform", IsFeatured = 1 },
            new { Name = "Task Management API", Description = "RESTful API for task management with real-time notifications, authentication, and comprehensive testing. Built with clean architecture principles.", Technologies = "ASP.NET Core, SignalR, JWT, xUnit, Swagger", StartDate = new DateTime(2021, 9, 1), EndDate = (DateTime?)new DateTime(2021, 11, 30), ProjectUrl = (string?)null, SourceCodeUrl = "https://github.com/johndoe/task-management-api", IsFeatured = 1 },
            new { Name = "Weather Dashboard", Description = "Interactive weather dashboard with real-time data visualization and location-based forecasts. Responsive design for mobile and desktop.", Technologies = "React, TypeScript, Chart.js, OpenWeather API, Tailwind CSS", StartDate = new DateTime(2021, 6, 1), EndDate = (DateTime?)new DateTime(2021, 7, 15), ProjectUrl = (string?)"https://weather-dashboard.example.com", SourceCodeUrl = "https://github.com/johndoe/weather-dashboard", IsFeatured = 0 },
            new { Name = "CurriculumVitae API", Description = "GraphQL API for managing CV data including education, skills, projects, and companies. Built with ASP.NET Core and GraphQL.", Technologies = "ASP.NET Core, GraphQL, SQLite, Entity Framework", StartDate = new DateTime(2023, 1, 1), EndDate = (DateTime?)null, ProjectUrl = (string?)null, SourceCodeUrl = "https://github.com/example/cv-api", IsFeatured = 1 }
        };

        foreach (var project in projects)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Project (Name, Description, Technologies, StartDate, EndDate, ProjectUrl, SourceCodeUrl, IsFeatured) 
                VALUES (@Name, @Description, @Technologies, @StartDate, @EndDate, @ProjectUrl, @SourceCodeUrl, @IsFeatured)";
            
            command.Parameters.AddWithValue("@Name", project.Name);
            command.Parameters.AddWithValue("@Description", project.Description);
            command.Parameters.AddWithValue("@Technologies", project.Technologies);
            command.Parameters.AddWithValue("@StartDate", project.StartDate);
            if (project.EndDate.HasValue)
                command.Parameters.AddWithValue("@EndDate", project.EndDate.Value);
            else
                command.Parameters.AddWithValue("@EndDate", DBNull.Value);
            
            if (project.ProjectUrl != null)
                command.Parameters.AddWithValue("@ProjectUrl", project.ProjectUrl);
            else
                command.Parameters.AddWithValue("@ProjectUrl", DBNull.Value);
            
            if (project.SourceCodeUrl != null)
                command.Parameters.AddWithValue("@SourceCodeUrl", project.SourceCodeUrl);
            else
                command.Parameters.AddWithValue("@SourceCodeUrl", DBNull.Value);
            command.Parameters.AddWithValue("@IsFeatured", project.IsFeatured);
            
            await command.ExecuteNonQueryAsync();
        }
    }

    private async Task SeedCompaniesAsync(SqliteConnection connection)
    {
        var companies = new[]
        {
            new { Name = "TechCorp Solutions", Description = "Leading technology solutions provider specializing in enterprise software development and cloud infrastructure.", Industry = "Technology", Location = "San Francisco, CA", Website = "https://techcorp-solutions.com", EmployeeCount = 2500, FoundedDate = new DateTime(2010, 3, 15), Notes = "Publicly traded company (NASDAQ: TCHS). Known for innovative microservices architecture solutions." },
            new { Name = "Digital Innovations Inc", Description = "Digital transformation consultancy helping businesses modernize their technology stack and processes.", Industry = "Consulting", Location = "New York, NY", Website = "https://digitalinnovations.com", EmployeeCount = 850, FoundedDate = new DateTime(2015, 7, 22), Notes = "Privately held. Specializes in React and .NET development. Acquired by Global Tech Partners in 2023." },
            new { Name = "Microsoft Corporation", Description = "Multinational technology corporation producing computer software, consumer electronics, and personal computers.", Industry = "Technology", Location = "Redmond, WA", Website = "https://microsoft.com", EmployeeCount = 221000, FoundedDate = new DateTime(1975, 4, 4), Notes = "Fortune 500 company. Major cloud computing provider with Azure platform." },
            new { Name = "Startup Ventures LLC", Description = "Early-stage technology startup focused on AI-powered business automation tools.", Industry = "Technology", Location = "Austin, TX", Website = "https://startupventures.io", EmployeeCount = 45, FoundedDate = new DateTime(2020, 11, 8), Notes = "Series A funding completed in 2022. Rapid growth in the automation sector." },
            new { Name = "OpenAI", Description = "AI research and deployment company focused on ensuring artificial general intelligence benefits all of humanity.", Industry = "Artificial Intelligence", Location = "San Francisco, CA", Website = "https://openai.com", EmployeeCount = 500, FoundedDate = new DateTime(2015, 12, 11), Notes = "Creator of ChatGPT and GPT models. Leading research in artificial intelligence." }
        };

        foreach (var company in companies)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Company (Name, Description, Industry, Location, Website, EmployeeCount, FoundedDate, Notes) 
                VALUES (@Name, @Description, @Industry, @Location, @Website, @EmployeeCount, @FoundedDate, @Notes)";
            
            command.Parameters.AddWithValue("@Name", company.Name);
            command.Parameters.AddWithValue("@Description", company.Description);
            command.Parameters.AddWithValue("@Industry", company.Industry);
            command.Parameters.AddWithValue("@Location", company.Location);
            command.Parameters.AddWithValue("@Website", company.Website);
            command.Parameters.AddWithValue("@EmployeeCount", company.EmployeeCount);
            command.Parameters.AddWithValue("@FoundedDate", company.FoundedDate);
            command.Parameters.AddWithValue("@Notes", company.Notes);
            
            await command.ExecuteNonQueryAsync();
        }
    }
} 