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
        var createPersonTable = @"
            CREATE TABLE IF NOT EXISTS Person (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                Email TEXT NOT NULL,
                Phone TEXT,
                Summary TEXT
            )";

        var createWorkExperienceTable = @"
            CREATE TABLE IF NOT EXISTS WorkExperience (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PersonId INTEGER NOT NULL,
                Company TEXT NOT NULL,
                Position TEXT NOT NULL,
                StartDate TEXT NOT NULL,
                EndDate TEXT,
                Description TEXT,
                FOREIGN KEY (PersonId) REFERENCES Person(Id)
            )";

        var createEducationTable = @"
            CREATE TABLE IF NOT EXISTS Education (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PersonId INTEGER NOT NULL,
                Institution TEXT NOT NULL,
                Degree TEXT NOT NULL,
                FieldOfStudy TEXT,
                StartDate TEXT NOT NULL,
                EndDate TEXT,
                Grade TEXT,
                Description TEXT,
                FOREIGN KEY (PersonId) REFERENCES Person(Id)
            )";

        var createSkillTable = @"
            CREATE TABLE IF NOT EXISTS Skill (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PersonId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                ProficiencyLevel INTEGER NOT NULL,
                Category INTEGER NOT NULL,
                YearsOfExperience INTEGER,
                Description TEXT,
                FOREIGN KEY (PersonId) REFERENCES Person(Id)
            )";

        var createProjectTable = @"
            CREATE TABLE IF NOT EXISTS Project (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PersonId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Description TEXT NOT NULL,
                Technologies TEXT,
                StartDate TEXT NOT NULL,
                EndDate TEXT,
                ProjectUrl TEXT,
                SourceCodeUrl TEXT,
                IsFeatured INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY (PersonId) REFERENCES Person(Id)
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
        
        command.CommandText = createPersonTable;
        await command.ExecuteNonQueryAsync();
        
        command.CommandText = createWorkExperienceTable;
        await command.ExecuteNonQueryAsync();
        
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
        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT COUNT(*) FROM Person";
        var personCount = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

        if (personCount == 0)
        {
            using var personCommand = connection.CreateCommand();
            personCommand.CommandText = @"
                INSERT INTO Person (FullName, Email, Phone, Summary) 
                VALUES (@FullName, @Email, @Phone, @Summary)";
            
            personCommand.Parameters.AddWithValue("@FullName", "John Doe");
            personCommand.Parameters.AddWithValue("@Email", "john.doe@example.com");
            personCommand.Parameters.AddWithValue("@Phone", "+1-555-123-4567");
            personCommand.Parameters.AddWithValue("@Summary", "Experienced software developer with expertise in .NET and modern web technologies.");
            
            await personCommand.ExecuteNonQueryAsync();

            using var getIdCommand = connection.CreateCommand();
            getIdCommand.CommandText = "SELECT last_insert_rowid()";
            var personId = Convert.ToInt32(await getIdCommand.ExecuteScalarAsync());

            await SeedWorkExperiencesAsync(connection, personId);
            
            await SeedEducationAsync(connection, personId);
            
            await SeedSkillsAsync(connection, personId);
            
            await SeedProjectsAsync(connection, personId);
        }

        // Always check and seed companies independently
        using var companyCheckCommand = connection.CreateCommand();
        companyCheckCommand.CommandText = "SELECT COUNT(*) FROM Company";
        var companyCount = Convert.ToInt32(await companyCheckCommand.ExecuteScalarAsync());

        if (companyCount == 0)
        {
            await SeedCompaniesAsync(connection);
        }
    }

    private async Task SeedWorkExperiencesAsync(SqliteConnection connection, int personId)
    {
        var workExperiences = new[]
        {
            new { Company = "TechCorp Solutions", Position = "Senior Software Engineer", StartDate = new DateTime(2021, 3, 1), EndDate = (DateTime?)null, Description = "Lead development of microservices architecture using .NET and Azure." },
            new { Company = "Digital Innovations Inc", Position = "Full Stack Developer", StartDate = new DateTime(2019, 1, 15), EndDate = (DateTime?)new DateTime(2021, 2, 28), Description = "Developed web applications using React and ASP.NET Core." }
        };

        foreach (var we in workExperiences)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO WorkExperience (PersonId, Company, Position, StartDate, EndDate, Description) 
                VALUES (@PersonId, @Company, @Position, @StartDate, @EndDate, @Description)";
            
            command.Parameters.AddWithValue("@PersonId", personId);
            command.Parameters.AddWithValue("@Company", we.Company);
            command.Parameters.AddWithValue("@Position", we.Position);
            command.Parameters.AddWithValue("@StartDate", we.StartDate);
            if (we.EndDate.HasValue)
                command.Parameters.AddWithValue("@EndDate", we.EndDate.Value);
            else
                command.Parameters.AddWithValue("@EndDate", DBNull.Value);
            command.Parameters.AddWithValue("@Description", we.Description);
            
            await command.ExecuteNonQueryAsync();
        }
    }

    private async Task SeedEducationAsync(SqliteConnection connection, int personId)
    {
        var educations = new[]
        {
            new { Institution = "University of Technology", Degree = "Bachelor of Science", FieldOfStudy = "Computer Science", StartDate = new DateTime(2012, 9, 1), EndDate = new DateTime(2016, 5, 15), Grade = "3.8 GPA", Description = "Graduated Magna Cum Laude. Relevant coursework: Data Structures, Algorithms, Software Engineering, Database Systems." },
            new { Institution = "Microsoft Learn", Degree = "Azure Developer Associate", FieldOfStudy = "Cloud Computing", StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 3, 15), Grade = "Certified", Description = "Earned AZ-204 certification demonstrating expertise in developing cloud solutions on Microsoft Azure platform." }
        };

        foreach (var edu in educations)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Education (PersonId, Institution, Degree, FieldOfStudy, StartDate, EndDate, Grade, Description) 
                VALUES (@PersonId, @Institution, @Degree, @FieldOfStudy, @StartDate, @EndDate, @Grade, @Description)";
            
            command.Parameters.AddWithValue("@PersonId", personId);
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

    private async Task SeedSkillsAsync(SqliteConnection connection, int personId)
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
            new { Name = "English", ProficiencyLevel = 10, Category = (int)SkillCategory.Language, YearsOfExperience = 25, Description = "Native speaker" }
        };

        foreach (var skill in skills)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Skill (PersonId, Name, ProficiencyLevel, Category, YearsOfExperience, Description) 
                VALUES (@PersonId, @Name, @ProficiencyLevel, @Category, @YearsOfExperience, @Description)";
            
            command.Parameters.AddWithValue("@PersonId", personId);
            command.Parameters.AddWithValue("@Name", skill.Name);
            command.Parameters.AddWithValue("@ProficiencyLevel", skill.ProficiencyLevel);
            command.Parameters.AddWithValue("@Category", skill.Category);
            command.Parameters.AddWithValue("@YearsOfExperience", skill.YearsOfExperience);
            command.Parameters.AddWithValue("@Description", skill.Description);
            
            await command.ExecuteNonQueryAsync();
        }
    }

    private async Task SeedProjectsAsync(SqliteConnection connection, int personId)
    {
        var projects = new[]
        {
            new { Name = "E-Commerce Platform", Description = "Full-stack e-commerce solution with microservices architecture, supporting 10,000+ concurrent users. Implemented using .NET 6, React, Redis, and PostgreSQL.", Technologies = "ASP.NET Core, React, TypeScript, Redis, PostgreSQL, Docker, Azure", StartDate = new DateTime(2022, 1, 1), EndDate = (DateTime?)new DateTime(2022, 8, 15), ProjectUrl = (string?)"https://demo-ecommerce.example.com", SourceCodeUrl = "https://github.com/johndoe/ecommerce-platform", IsFeatured = 1 },
            new { Name = "Task Management API", Description = "RESTful API for task management with real-time notifications, authentication, and comprehensive testing. Built with clean architecture principles.", Technologies = "ASP.NET Core, SignalR, JWT, xUnit, Swagger", StartDate = new DateTime(2021, 9, 1), EndDate = (DateTime?)new DateTime(2021, 11, 30), ProjectUrl = (string?)null, SourceCodeUrl = "https://github.com/johndoe/task-management-api", IsFeatured = 1 },
            new { Name = "Weather Dashboard", Description = "Interactive weather dashboard with real-time data visualization and location-based forecasts. Responsive design for mobile and desktop.", Technologies = "React, TypeScript, Chart.js, OpenWeather API, Tailwind CSS", StartDate = new DateTime(2021, 6, 1), EndDate = (DateTime?)new DateTime(2021, 7, 15), ProjectUrl = (string?)"https://weather-dashboard.example.com", SourceCodeUrl = "https://github.com/johndoe/weather-dashboard", IsFeatured = 0 }
        };

        foreach (var project in projects)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Project (PersonId, Name, Description, Technologies, StartDate, EndDate, ProjectUrl, SourceCodeUrl, IsFeatured) 
                VALUES (@PersonId, @Name, @Description, @Technologies, @StartDate, @EndDate, @ProjectUrl, @SourceCodeUrl, @IsFeatured)";
            
            command.Parameters.AddWithValue("@PersonId", personId);
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
            new { Name = "Startup Ventures LLC", Description = "Early-stage technology startup focused on AI-powered business automation tools.", Industry = "Technology", Location = "Austin, TX", Website = "https://startupventures.io", EmployeeCount = 45, FoundedDate = new DateTime(2020, 11, 8), Notes = "Series A funding completed in 2022. Rapid growth in the automation sector." }
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