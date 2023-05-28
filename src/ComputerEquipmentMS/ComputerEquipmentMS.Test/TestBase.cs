using System.Text;
using Testcontainers.PostgreSql;

namespace ComputerEquipmentMS.Test;

public abstract class TestBase : IAsyncLifetime
{
    protected readonly PostgreSqlContainer Container;
    protected string ConnectionString { get; }

    protected TestBase()
    {
        Container = new PostgreSqlBuilder()
            .WithDatabase("computer_equipment_ms")
            .WithUsername("admin")
            .WithPassword("admin")
            .Build();
        
        Container.StartAsync().Wait();
        ConnectionString = $"{Container.GetConnectionString()};Include Error Detail=true";

        InitDb().Wait();
    }

    public virtual Task InitializeAsync() => 
        Task.CompletedTask;

    public virtual Task DisposeAsync() => 
        Container.DisposeAsync().AsTask();


    private Task InitDb()
    {
        var scriptBuilder = new StringBuilder();
        
        var files = Directory.EnumerateFiles(GetInitDbScriptsPath());
        foreach (var file in files)
        {
            var fileContent = File.ReadAllText(file);
            scriptBuilder.AppendLine(fileContent);
        }

        var script = scriptBuilder.ToString();
        
        return Container.ExecScriptAsync(script);
    }

    private static string GetInitDbScriptsPath()
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (currentDirectory is not null && !currentDirectory.GetFiles("*.sln").Any())
            currentDirectory = currentDirectory.Parent;

        return $"{currentDirectory}/ComputerEquipmentMS/Initdb";
    }
}